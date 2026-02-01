namespace Inventory_Management_System.DTOs
{
    public class PurchaseDto
    {
        public int id { get; set; }

        public string item_name { get; set; } = null!;

        public string? description { get; set; }

        public int quantity_bought { get; set; }

        public decimal price { get; set; }

        public decimal total_price { get; set; }

        public DateTime purchase_date { get; set; }

        public int userId { get; set; }

        public string buyer_name { get; set; } = "Unknown";
    }
}
