using Inventory_Management_System.Models;

public interface ICartRepository
{
    Task<Cart?> GetActiveCart(int userId);

    Task<Cart?> GetCartWithItems(int userId);

    Task AddCart(Cart cart);

    Task<CartItem?> GetCartItem(int cartId, int storeItemId);

    Task<CartItem?> GetCartItemById(int cartItemId);

    Task AddCartItem(CartItem cartItem);

    void RemoveCartItem(CartItem cartItem);

    Task Save();
}
