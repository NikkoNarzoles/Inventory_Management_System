using Inventory_Management_System.Models.StoreModels;
using Inventory_Management_System.ViewModels;

namespace Inventory_Management_System.Repositories.Interfaces
{
    public interface IPurchaseRepository
    {
        Task<IEnumerable<Purchase>> ListAllAsync();
        Task AddAsync(Purchase purchase);
        Task DeleteAsync(int id);
        Task UpdateAsync(Purchase purchase);
        Task<Purchase?> FindAsync(int id);

        Task<IEnumerable<Purchase>> GetPurchasesByOwnerAsync(int ownerId);

        Task<IEnumerable<Purchase>> GetOwnPurchaseAsync(int buyersId);
    }
}
