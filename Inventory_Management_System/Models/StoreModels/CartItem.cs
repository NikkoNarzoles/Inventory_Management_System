using Inventory_Management_System.Models.StoreModels;

public class CartItem
{
    public int Id { get; set; }

    public int CartId { get; set; }
    public Cart Cart { get; set; } = null!;

    public int StoreItemId { get; set; }
    public StoreItem StoreItem { get; set; } = null!;

    public int Quantity { get; set; }

    // ⭐ VERY IMPORTANT
    public decimal UnitPrice { get; set; }

    // ⭐ allows selected checkout
    public bool IsSelected { get; set; } = false;
}
