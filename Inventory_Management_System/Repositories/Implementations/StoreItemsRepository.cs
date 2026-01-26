using Inventory_Management_System.Data;
using Inventory_Management_System.DTOs;
using Inventory_Management_System.Models;
using Inventory_Management_System.Repositories.Interfaces;
using Inventory_Management_System.ViewModels;
using Microsoft.EntityFrameworkCore;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;



namespace Inventory_Management_System.Repositories.Implementations
{
    public class StoreItemsRepository : IStoreItemsRepository
    {

        private readonly InventoryDbContext _context;

        public StoreItemsRepository(InventoryDbContext context)
        {
            _context = context;
        }



        // GET BY ID (async)
        public async Task<BuyViewModel> NoCodeListAsync(int id)
        {
            return await _context.StoreItems
                .AsNoTracking()
                .Where(item => item.id == id)
                .Select(item => new BuyViewModel
                {
                    id = item.id,
                    item_name = item.item_name,
                    description = item.description,
                    quan = item.quantity,
                    price = item.price
                })
                .FirstAsync();

        }


        //=================================================================================================================
        //=================================================================================================================



        // GET ALL (async)

        public async Task<List<StoreItemsDto>> GetAllAsync()
        {
            return await _context.StoreItems
                .AsNoTracking()
                .Select(item => new StoreItemsDto
                {
                    id = item.id,
                    item_code = item.item_code,
                    item_name = item.item_name,
                    description = item.description,
                    quantity = item.quantity,
                    price = item.price
                })
                .ToListAsync();
        }

        public async Task<List<StoreItemsDto>> SearchAsync(string? search)
        {
            IQueryable<StoreItem> query =
                _context.StoreItems.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(i =>
                    i.item_name.Contains(search) ||
                    i.item_code.Contains(search)
                );
            }

            return await query
                .Select(item => new StoreItemsDto
                {
                    id = item.id,
                    item_code = item.item_code,
                    item_name = item.item_name,
                    description = item.description,
                    quantity = item.quantity,
                    price = item.price
                })
                .ToListAsync();
        }




        //=================================================================================================================
        //=================================================================================================================



        // GET BY ID (async)
        public async Task<StoreItemsDto?> GetByIdAsync(int id)
        {
            return await _context.StoreItems
                .AsNoTracking()
                .Where(item => item.id == id)
                .Select(item => new StoreItemsDto
                {
                    id = item.id,
                    item_code = item.item_code,
                    item_name = item.item_name,
                    description = item.description,
                    quantity = item.quantity,
                    price = item.price
                })
                .FirstOrDefaultAsync();
        }




        //=================================================================================================================
        //=================================================================================================================


        public async Task<StoreItem?> FindAsync(int id)
    {

        return await _context.StoreItems
           .FindAsync(id);
    }




        //=================================================================================================================
        //=================================================================================================================





        // ADD (async)
        public async Task AddAsync(StoreItem item)
    {
        await _context.StoreItems.AddAsync(item);
        await _context.SaveChangesAsync();
    }



        //=================================================================================================================
        //=================================================================================================================



        // UPDATE (async)
        public async Task UpdateAsync(StoreItem item)
    {
        _context.StoreItems.Update(item);
        await _context.SaveChangesAsync();
    }



        //=================================================================================================================
        //=================================================================================================================


        // DELETE (async)
        public async Task DeleteAsync(int id)
    {
        var item = await _context.StoreItems.FindAsync(id);
        if (item != null)
        {
            _context.StoreItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
}
