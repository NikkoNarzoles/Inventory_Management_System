using Inventory_Management_System.Data;
using Inventory_Management_System.Models.StoreModels;
using Microsoft.EntityFrameworkCore;

public class CartRepository : ICartRepository
{
    private readonly InventoryDbContext _context;

    public CartRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetActiveCart(int userId)
    {
        return await _context.Carts
            .FirstOrDefaultAsync(c =>
                c.UserId == userId &&
                c.Status == CartStatus.Active);
    }

    public async Task<Cart?> GetCartWithItems(int userId)
    {
        return await _context.Carts
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.StoreItem)
            .FirstOrDefaultAsync(c =>
                c.UserId == userId &&
                c.Status == CartStatus.Active);
    }

    public async Task AddCart(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
    }

    public async Task<CartItem?> GetCartItem(int cartId, int storeItemId)
    {
        return await _context.CartItems
            .FirstOrDefaultAsync(ci =>
                ci.CartId == cartId &&
                ci.StoreItemId == storeItemId);
    }

    public async Task<CartItem?> GetCartItemById(int cartItemId)
    {
        return await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId);
    }

    public async Task AddCartItem(CartItem cartItem)
    {
        await _context.CartItems.AddAsync(cartItem);
    }

    public void RemoveCartItem(CartItem cartItem)
    {
        _context.CartItems.Remove(cartItem);
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}
