namespace Inventory_Management_System.Services.ServiceInterface
{
    public interface ICartService
    {
        Task AddToCart(int userId, int storeItemId);

        Task UpdateQuantity(int cartItemId, int quantity);

        Task ToggleSelect(int cartItemId, bool isSelected);

        Task RemoveItem(int cartItemId);

        Task<Cart?> GetActiveCart(int userId);
    }


}
