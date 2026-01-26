namespace Inventory_Management_System.Models
{
    public class Purchase
    {

        public int id { get; set; }
        public string item_name { get; set; } = null!;

        public string? description { get; set; }
        public int quantity_bought { get; set; } = 1;
        public decimal price { get; set; }
        public decimal total_price { get; set; }
        public DateTime purchase_date { get; set; }

        public int user_id { get; set; } 

    }
}
