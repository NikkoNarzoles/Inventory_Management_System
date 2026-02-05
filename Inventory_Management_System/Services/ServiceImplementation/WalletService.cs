using Inventory_Management_System.Data;
using Inventory_Management_System.Models;
using Inventory_Management_System.Models.StoreModels;
using Inventory_Management_System.Models.TransactionModel;
using Inventory_Management_System.Services.ServiceInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

public class WalletService : IWalletService
{
    private readonly InventoryDbContext _context;
    private readonly IHttpContextAccessor _http;

    public WalletService(InventoryDbContext context, IHttpContextAccessor http)
    {
        _context = context;
        _http = http;
    }

    // =====================================================
    // 🔥 BALANCE (LEDGER SOURCE OF TRUTH)
    // =====================================================
    public async Task<decimal> GetWalletBalance(int userId)
    {
        var wallet = await _context.Wallets
            .FirstOrDefaultAsync(w => w.UserId == userId);

        if (wallet == null)
            return 0m;

        return await GetWalletBalanceById(wallet.Id);
    }

    private async Task<decimal> GetWalletBalanceById(int walletId)
    {
        return await _context.LedgerEntries
            .Where(l => l.WalletId == walletId)
            .SumAsync(l => (decimal?)l.Amount) ?? 0m;
    }

    // =====================================================
    // 🔥 CREATE WALLET
    // =====================================================
    public async Task<int> CreateWalletAsync(int userId)
    {
        var wallet = new Wallets
        {
            UserId = userId,
            WalletType = "User",
            Status = "Active",
            CreatedAt = DateTime.UtcNow
        };

        _context.Wallets.Add(wallet);
        await _context.SaveChangesAsync();

        return wallet.Id;
    }

    // =====================================================
    // 🔥 PRIVATE CORE ENGINE (LOW LEVEL)
    // DEV + INTERNAL USE ONLY
    // =====================================================
    private async Task CreateTransactionAsync(
        int fromWalletId,
        int toWalletId,
        decimal amount,
        int actingUserId)
    {
        using var dbTransaction =
            await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        try
        {
            if (amount <= 0)
                throw new Exception("Invalid amount");

            if (fromWalletId == toWalletId)
                throw new Exception("Invalid transfer: same wallet");

            var fromWallet = await _context.Wallets.FindAsync(fromWalletId);
            var toWallet = await _context.Wallets.FindAsync(toWalletId);

            if (fromWallet == null || toWallet == null)
                throw new Exception("Wallet not found");

            var currentBalance = await GetWalletBalanceById(fromWalletId);

            if (currentBalance < amount)
                throw new Exception("Insufficient balance");

            // 🔥 TRANSACTION RECORD
            var transaction = new Transactions
            {
                ReferenceNo = Guid.NewGuid().ToString(),
                Type = "Transfer",
                Status = "Completed",
                Amount = amount,
                CreatedByUserId = Guid.NewGuid(), // keep Guid if your DB expects it
                CreatedAt = DateTime.UtcNow
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            // 🔥 DOUBLE ENTRY LEDGER
            var debitEntry = new LedgerEntries
            {
                TransactionId = transaction.Id,
                WalletId = fromWalletId,
                Amount = -amount,
                EntryType = "Debit",
                CreatedAt = DateTime.UtcNow
            };

            var creditEntry = new LedgerEntries
            {
                TransactionId = transaction.Id,
                WalletId = toWalletId,
                Amount = amount,
                EntryType = "Credit",
                CreatedAt = DateTime.UtcNow
            };

            _context.LedgerEntries.AddRange(debitEntry, creditEntry);

            // 🔥 AUDIT LOG
            _context.AuditLogs.Add(new AuditLogs
            {
                UserId = null,
                Action = "WALLET_TRANSFER",
                EntityName = "Wallet",
                EntityId = fromWalletId,
                Timestamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            await dbTransaction.CommitAsync();
        }
        catch
        {
            await dbTransaction.RollbackAsync();
            throw;
        }
    }

    // =====================================================
    // 🔥 SAFE USER TRANSFER (PUBLIC ENTRY POINT)
    // =====================================================
    public async Task CreateTransactionByUserAsync(
        int fromUserId,
        string fromWalletType,
        int toUserId,
        string toWalletType,
        decimal amount)
    {
        // 🔐 AUTHORIZATION CHECK
        var currentUserIdString =
            _http.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentUserIdString))
            throw new Exception("Unauthorized");

        int currentUserId = int.Parse(currentUserIdString);

        if (currentUserId != fromUserId)
            throw new Exception("Unauthorized wallet access");

        var fromWallet = await _context.Wallets
            .FirstOrDefaultAsync(w =>
                w.UserId == fromUserId &&
                w.WalletType == fromWalletType);

        var toWallet = await _context.Wallets
            .FirstOrDefaultAsync(w =>
                w.UserId == toUserId &&
                w.WalletType == toWalletType);

        if (fromWallet == null || toWallet == null)
            throw new Exception("Wallet not found");

        // 🔥 CALL PRIVATE ENGINE
        await CreateTransactionAsync(
            fromWallet.Id,
            toWallet.Id,
            amount,
            fromUserId);
    }

    // =====================================================
    // 🔥 ADMIN TEST SEED
    // =====================================================
    public async Task SeedAdminBalanceAsync(int adminUserId, decimal amount)
    {
        if (amount <= 0)
            throw new Exception("Invalid amount");

        var adminWallet = await _context.Wallets
            .FirstOrDefaultAsync(w =>
                w.UserId == adminUserId &&
                w.WalletType == "User");

        if (adminWallet == null)
            throw new Exception("Admin wallet not found");

        var transaction = new Transactions
        {
            ReferenceNo = Guid.NewGuid().ToString(),
            Type = "TestSeed",
            Status = "Completed",
            Amount = amount,
            CreatedByUserId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        _context.LedgerEntries.Add(new LedgerEntries
        {
            TransactionId = transaction.Id,
            WalletId = adminWallet.Id,
            Amount = amount,
            EntryType = "Credit",
            Description = "TEST ADMIN BALANCE",
            CreatedAt = DateTime.UtcNow
        });

        _context.AuditLogs.Add(new AuditLogs
        {
            UserId = null,
            Action = "ADMIN_SEED",
            EntityName = "Wallet",
            EntityId = adminWallet.Id,
            Timestamp = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
    }

    // =====================================================
    // 🔥 PUBLIC BALANCE API
    // =====================================================
    public async Task<decimal> GetUserWalletBalanceAsync(int userId, string walletType)
    {
        var wallet = await _context.Wallets
            .FirstOrDefaultAsync(w => w.UserId == userId && w.WalletType == walletType);

        if (wallet == null)
            return 0m;

        return await GetWalletBalanceById(wallet.Id);
    }
}
