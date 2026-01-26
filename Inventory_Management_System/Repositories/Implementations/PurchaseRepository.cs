using Inventory_Management_System.Data;
using Inventory_Management_System.Models;
using Inventory_Management_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management_System.Repositories.Implementations
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly InventoryDbContext _context;
     

        public PurchaseRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Purchase>> ListAllAsync()
        {
            return await _context.Purchases
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Purchase purchase)
        {
            await _context.Purchases.AddAsync(purchase);
            await _context.SaveChangesAsync();
        }

        public async Task<Purchase?> FindAsync(int id)
        {
            return await _context.Purchases.FindAsync(id);
        }

        public async Task UpdateAsync(Purchase purchase)
        {
            _context.Purchases.Update(purchase);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var purchase = await _context.Purchases.FindAsync(id);

            if (purchase != null)
            {
                _context.Purchases.Remove(purchase);
                await _context.SaveChangesAsync();
            }
        }
    }
}
