using System.ComponentModel.DataAnnotations;

namespace Inventory_Management_System.ViewModels
{
    public class CheckoutIndexViewModel
    {
        public List<CheckoutItemVM> Items { get; set; } = new();

        public decimal TotalAmount { get; set; }
    }

    public class CheckoutItemVM
    {
        public int CartItemId { get; set; }

        public string ItemName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        [Range(1, 9999, ErrorMessage = "Invalid quantity")]
        public int InputQuantity { get; set; }
    }
}
