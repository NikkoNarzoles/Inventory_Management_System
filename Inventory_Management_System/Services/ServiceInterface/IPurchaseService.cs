using Inventory_Management_System.DTOs;
using Inventory_Management_System.ViewModels;
using Inventory_Management_System.Models.StoreModels;

namespace Inventory_Management_System.Services.Interfaces
{
    public interface IPurchaseService
    {
        Task<int> CreatePurchaseAsync(Purchase purchase);
        Task<Purchase?> GetPurchaseAsync(int id);
        Task<IEnumerable<Purchase>> ListPurchasesAsync();
        BuyViewModel MapToBuyViewModel(Purchase purchase);
        IEnumerable<PurchaseDto> MapToPurchaseViewModels(IEnumerable<Purchase> purchases,IEnumerable<UserDto>? users =null);
        PurchaseDto MapToPurchaseViewModel(Purchase purchase);
        Task DeletePurchaseAsync(int id);


    }
}
