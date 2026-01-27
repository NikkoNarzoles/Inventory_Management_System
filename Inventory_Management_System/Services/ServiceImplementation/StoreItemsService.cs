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



        public async Task<StoreItemsDto> EditMapAsync(int id)
        {
            var item = await _IStoreRepo.GetByIdAsync(id);

            if (item == null)
                return null!;

            return new StoreItemsDto
            {
                item_code = item.item_code,
                item_name = item.item_name,
                description = item.description,
                quantity = item.quantity,
                price = item.price,
            };
        }


         public async Task<bool> EditAsync(StoreItemsViewModels viewModel, int id)
        {
            var item = await _IStoreRepo.FindAsync(id);

            if (item == null)
                return false;

            item.item_code = viewModel.item_code;
            item.item_name = viewModel.item_name;
            item.description = viewModel.description;
            item.quantity = viewModel.quantity;
            item.price = viewModel.price;
            item.supplier = viewModel.supplier;
            item.updated_at = DateTime.UtcNow;

            await _IStoreRepo.UpdateAsync(item);
            return true;
        }








        public async Task CreateAsync(StoreItemsViewModels viewModel, int userId)
        {
            var item = new StoreItem
            {
                item_code = viewModel.item_code,
                item_name = viewModel.item_name,
                description = viewModel.description,
                quantity = viewModel.quantity,
                price = viewModel.price,
                supplier = viewModel.supplier,

                owners_id = userId,
                created_at = DateTime.UtcNow
            };

            await _IStoreRepo.AddAsync(item);
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
                owners_id = item.owners_id,

                user_id = userId
            };

            var purchaseId = await _purchaseService.CreatePurchaseAsync(purchase);

          
            return purchaseId;
        }


        public async Task<List<StoreItemsDto>> GetItemsAsync(string? search,StoreItemSortBy? sortBy)
        {
            return await _IStoreRepo.GetAsync(search, sortBy);
        }




    }
}
