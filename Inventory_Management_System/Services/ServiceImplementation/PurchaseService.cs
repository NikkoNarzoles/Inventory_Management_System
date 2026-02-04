using Inventory_Management_System.DTOs;
using Inventory_Management_System.Models;
using Inventory_Management_System.Repositories.Interfaces;
using Inventory_Management_System.Services.Interfaces;
using Inventory_Management_System.ViewModels;
using System.Security.Claims;

namespace Inventory_Management_System.Services.Implementations
{
    public class PurchaseService :  IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepo;

       

        public PurchaseService(IPurchaseRepository purchaseRepo)
        {
            _purchaseRepo = purchaseRepo;

           
        }   

       
        public PurchaseDto MapToPurchaseViewModel(Purchase purchase)
        {
            return new PurchaseDto
            {
                id = purchase.id,
                item_name = purchase.item_name,
                quantity_bought = purchase.quantity_bought,
                total_price = purchase.total_price,
                purchase_date = purchase.purchase_date,
                userId = purchase.user_id,

            };
        }

        public IEnumerable<PurchaseDto> MapToPurchaseViewModels(IEnumerable<Purchase> purchases, IEnumerable<UserDto> users)
        {
            return purchases.Select(p => {
                                                var user = users.FirstOrDefault(u => u.id == p.user_id);

                                                return new PurchaseDto
                                                {
                                                    id = p.id,
                                                    item_name = p.item_name,
                                                    quantity_bought = p.quantity_bought,
                                                    total_price = p.total_price,
                                                    purchase_date = p.purchase_date,
                                                    userId = p.user_id,
                                                   

                                                    buyer_name = user != null
                                                            ? $"{user.first_name} {user.last_name}"
                                                            : "Unknown"
                                                            };
                                            }
                                          );

        }






        public async Task<int>  CreatePurchaseAsync(Purchase purchase)
        {
            purchase.purchase_date = DateTime.UtcNow;

            await _purchaseRepo.AddAsync(purchase);

            return purchase.id;
        }


        public async Task<Purchase?> GetPurchaseAsync(int id)
        {
            return await _purchaseRepo.FindAsync(id);
        }


        public async Task<IEnumerable<Purchase>> ListPurchasesAsync()
        {
            return await _purchaseRepo.ListAllAsync();
        }

        public BuyViewModel MapToBuyViewModel(Purchase purchase)
        {
            return new BuyViewModel
            {
                item_name = purchase.item_name,
                description = purchase.description,
                quan = purchase.quantity_bought,
                price = purchase.price,
                total_price = purchase.total_price
            };
        }


        public async Task DeletePurchaseAsync(int id)
        {
            await _purchaseRepo.DeleteAsync(id);
        }


    }
}
