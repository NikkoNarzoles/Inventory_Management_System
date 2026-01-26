using Inventory_Management_System.DTOs;
using Inventory_Management_System.Models;
using Inventory_Management_System.ViewModels;

namespace Inventory_Management_System.Repositories.Interfaces
{
    public interface IStoreItemsRepository
    {
        Task<IEnumerable<StoreItemsDto>> GetAllAsync();
        Task<StoreItemsDto?> GetByIdAsync(int id);
        Task<StoreItem?> FindAsync(int id);
        Task AddAsync(StoreItem item);
        Task UpdateAsync(StoreItem item);
        Task DeleteAsync(int id);
        Task<BuyViewModel> NoCodeListAsync(int id);
    }
}
