using Inventory_Management_System.Models;
using Inventory_Management_System.Repositories.Interfaces;
using Inventory_Management_System.Services.ServiceInterface;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepo;
    private readonly IStoreItemsRepository _itemRepo;

    public CartService(ICartRepository cartRepo,
                       IStoreItemsRepository itemRepo)
    {
        _cartRepo = cartRepo;
        _itemRepo = itemRepo;
    }




    public async Task UpdateQuantity(int cartItemId, int quantity)
    {
        var cartItem = await _cartRepo.GetCartItemById(cartItemId);

        if (cartItem == null) return;

        if (quantity <= 0)
        {
            _cartRepo.RemoveCartItem(cartItem);
        }
        else
        {
            cartItem.Quantity = quantity;
        }

        await _cartRepo.Save();
    }





    public async Task<Cart?> GetActiveCart(int userId)
    {
        return await _cartRepo.GetCartWithItems(userId);
    }
    





    public async Task AddToCart(int userId, int storeItemId)
    {
        var cart = await _cartRepo.GetActiveCart(userId);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId,
                Status = CartStatus.Active
            };

            await _cartRepo.AddCart(cart);
            await _cartRepo.Save();
        }

        var existing = await _cartRepo.GetCartItem(cart.Id, storeItemId);

        if (existing != null)
        {
            existing.Quantity++;
        }
        else
        {
            var item = await _itemRepo.FindAsync(storeItemId);

            if (item == null) return;

            await _cartRepo.AddCartItem(new CartItem
            {
                CartId = cart.Id,
                StoreItemId = storeItemId,
                Quantity = 1,
                UnitPrice = item.price,
                IsSelected = false
            });
        }

        await _cartRepo.Save();
    }






    public async Task ToggleSelect(int cartItemId, bool selected)
    {
        var cartItem = await _cartRepo.GetCartItemById(cartItemId);

        if (cartItem == null) return;

        cartItem.IsSelected = selected;

        await _cartRepo.Save();
    }





    public async Task RemoveItem(int cartItemId)
    {
        var cartItem = await _cartRepo.GetCartItemById(cartItemId);

        if (cartItem == null) return;

        _cartRepo.RemoveCartItem(cartItem);

        await _cartRepo.Save();
    }
}
