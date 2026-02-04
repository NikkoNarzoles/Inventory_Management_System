using System.ComponentModel.DataAnnotations;

namespace Inventory_Management_System.ViewModels
{
    public class CartViewModel
    {
        public List<CartItemVM> Items { get; set; } = new();
    }

    public class CartItemVM
    {
        public int CartItemId { get; set; }

        public string ItemName { get; set; } = string.Empty;

        [Range(1, 9999, ErrorMessage = "Invalid quantity")]
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public bool IsSelected { get; set; }
    }
}
