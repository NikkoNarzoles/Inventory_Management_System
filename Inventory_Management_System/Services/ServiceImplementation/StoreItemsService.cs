using Inventory_Management_System.Models;
using Inventory_Management_System.Repositories.Interfaces;
using Inventory_Management_System.Services.Interfaces;
using Inventory_Management_System.Services.ServiceInterface;
using Inventory_Management_System.ViewModels;
using Microsoft.EntityFrameworkCore;
using Inventory_Management_System.DTOs;



namespace Inventory_Management_System.Services.ServiceImplementation
{
    public class StoreItemsService : IStoreItemsService
    {

        private readonly IStoreItemsRepository _IStoreRepo;
        private readonly IPurchaseService _purchaseService;


        public StoreItemsService(
                IStoreItemsRepository storeRepo,
                IPurchaseService purchaseService)
        {
            _IStoreRepo = storeRepo;
            _purchaseService = purchaseService;
        }



        public async Task<BuyViewModel> Buymap(int id, int quan)
        {
            if (quan <= 0)
                throw new Exception("Invalid quantity");

            var item = await _IStoreRepo.FindAsync(id);

            if (item == null)
                throw new Exception("Item not found");

            return new BuyViewModel
            {
                id = item.id,
                item_name = item.item_name!,
                description = item.description,
                quan = quan,
                price = item.price,
                total_price = item.price * quan,
            };

        }


        public async Task<BuyViewModel> Buypost (Purchase purchase)
        {
            var vm = new BuyViewModel
            {
                item_name = purchase.item_name,
                description = purchase.description,
                quan = purchase.quantity_bought,
                price = purchase.price,
                total_price = purchase.total_price
            };

            return (vm);
        }



        public async Task<int> BuyConfirmAsync(int id, int quan, int userId)
        {
            if (quan <= 0)
                throw new Exception("Invalid quantity");

            var item = await _IStoreRepo.FindAsync(id);

            if (item == null)
                throw new Exception("Item not found");

            if (item.quantity < quan)
                throw new Exception("Insufficient stock");

            item.quantity -= quan;
            await _IStoreRepo.UpdateAsync(item);

            var purchase = new Purchase
            {
                item_name = item.item_name!,
                description = item.description,
                quantity_bought = quan,
                price = item.price,
                total_price = item.price * quan,

                user_id = userId
            };

            var purchaseId = await _purchaseService.CreatePurchaseAsync(purchase);

          
            return purchaseId;
        }


        public async Task<List<StoreItemsDto>> GetItemsAsync(string? search)
        {
            return string.IsNullOrWhiteSpace(search)
                ? await _IStoreRepo.GetAllAsync()
                : await _IStoreRepo.SearchAsync(search);
        }


        public

    }
}
