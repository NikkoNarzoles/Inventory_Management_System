using Inventory_Management_System.Models;
using Inventory_Management_System.ViewModels;

namespace Inventory_Management_System.Services.ServiceInterface
{
    public interface IStoreItemsService
    {

        Task<BuyViewModel> Buymap(int id, int quan);

        Task<int> BuyConfirmAsync(int id, int quan, int userId);

        
    }   
}
