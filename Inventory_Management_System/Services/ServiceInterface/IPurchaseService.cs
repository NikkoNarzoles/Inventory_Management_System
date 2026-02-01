using Inventory_Management_System.Models;
using Inventory_Management_System.DTOs;
using Inventory_Management_System.ViewModels;

namespace Inventory_Management_System.Services.Interfaces
{
    public interface IPurchaseService
    {
        Task<int> CreatePurchaseAsync(Purchase purchase);
        Task<Purchase?> GetPurchaseAsync(int id);
        Task<IEnumerable<Purchase>> ListPurchasesAsync();
        BuyViewModel MapToBuyViewModel(Purchase purchase);
        IEnumerable<PurchaseDto> MapToPurchaseViewModels(IEnumerable<Purchase> purchases,IEnumerable<UserDto> users);
        PurchaseDto MapToPurchaseViewModel(Purchase purchase);
        Task DeletePurchaseAsync(int id);


    }
}
