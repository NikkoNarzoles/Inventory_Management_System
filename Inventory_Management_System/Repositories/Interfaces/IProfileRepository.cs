using Inventory_Management_System.DTOs;

namespace Inventory_Management_System.Repositories.Interfaces
{
    public interface IProfileRepository
    {
        Task<IEnumerable<StoreItemsDto>> GetOwnItems(int userId);
    }
}
