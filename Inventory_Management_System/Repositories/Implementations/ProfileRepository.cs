using Inventory_Management_System.Data;
using Inventory_Management_System.DTOs;
using Inventory_Management_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management_System.Repositories.Implementations
{
    public class ProfileRepository : IProfileRepository
    {

       
        private readonly InventoryDbContext _context;

        public ProfileRepository(InventoryDbContext inventoryDb)
        {
            _context = inventoryDb;
        }




        public async Task<IEnumerable<StoreItemsDto>> GetOwnItems (int userId)
        {
            return await _context.StoreItems
                        .AsNoTracking()
                        .Where(item => item.owners_id == userId)
                        .Select(
                         item => new StoreItemsDto
                         {
                             id = item.id,
                             item_name = item.item_name,
                             description  = item.description,
                             price = item.price,
                             quantity = item.quantity
                         }
                        ).ToListAsync();
        }






    }
}
