using Inventory_Management_System.Models.StoreModels;
using Inventory_Management_System.Repositories.Interfaces;
using Inventory_Management_System.Services.Implementations;
using Inventory_Management_System.Services.Interfaces;
using Inventory_Management_System.Services.ServiceInterface;

namespace Inventory_Management_System.Services.ServiceImplementation
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IStoreItemsRepository _storeRepo;
        private readonly IPurchaseService _purchaseService;


        public CheckoutService(ICartRepository cartRepo,
                               IOrderRepository orderRepo,
                               IStoreItemsRepository storeRepo,
                               IPurchaseService  purchaseService)
        {
            _cartRepo = cartRepo;
            _orderRepo = orderRepo;
            _storeRepo = storeRepo;
            _purchaseService = purchaseService;
        }

        public async Task<int?> CheckoutSelectedItems(int userId)
        {
            var cart = await _cartRepo.GetCartWithItems(userId);

            if (cart == null) return null;

            var selected = cart.CartItems
                .Where(ci => ci.IsSelected)
                .ToList();

            if (!selected.Any()) return null;

            var order = new Order
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            decimal total = 0;

            foreach (var ci in selected)
            {
                var storeItem = await _storeRepo.FindAsync(ci.StoreItemId);

                if (storeItem == null)
                    throw new Exception("Item not found");

                if (storeItem.quantity < ci.Quantity)
                    throw new Exception($"Insufficient stock for {storeItem.item_name}");

                // ⭐ Deduct stock
                storeItem.quantity -= ci.Quantity;


                var purchase = new Purchase
                {
                    item_name = storeItem.item_name!,
                    description = storeItem.description,
                    quantity_bought = ci.Quantity,
                    price = storeItem.price,
                    total_price = storeItem.price * ci.Quantity,
                    owners_id = storeItem.owners_id,
                    user_id = userId
                };
                
                await _purchaseService.CreatePurchaseAsync(purchase);


                // ⭐ Save update
                await _storeRepo.UpdateAsync(storeItem);


                order.OrderItems.Add(new OrderItem
                {
                    StoreItemId = ci.StoreItemId,
                    ItemName = ci.StoreItem.item_name,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.UnitPrice
                });

                total += ci.Quantity * ci.UnitPrice;
            }

            order.TotalAmount = total;

            await _orderRepo.AddOrder(order);

            foreach (var ci in selected)
            {
                _cartRepo.RemoveCartItem(ci);
            }

            await _orderRepo.Save();

            return order.Id;
        }



    }
}
