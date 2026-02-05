using Inventory_Management_System.Models.StoreModels;

namespace Inventory_Management_System.Models.TransactionModel
{
    public class LedgerEntries
    {
        public int Id { get; set; }

        public int TransactionId { get; set; }

        public int WalletId { get; set; }

        public decimal Amount { get; set; }          // +credit / -debit

        public string EntryType { get; set; } = null!; // Credit or Debit

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Transactions? Transaction { get; set; }

        public Wallets? Wallet { get; set; }
    }
}
