using Inventory_Management_System.Models;

public class Cart
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public CartStatus Status { get; set; } = CartStatus.Active;

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
    