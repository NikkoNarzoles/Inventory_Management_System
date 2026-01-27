    namespace Inventory_Management_System.Models
{
    public class StoreItem
    {

        public int id { get; set; }

        public required string item_code { get; set; }
        public required string item_name { get; set; }
        public string? description { get; set; }

        public int quantity { get; set; }

        public decimal price { get; set; }

        public string? supplier { get; set; }

        public int owners_id { get; set; }

        public DateTime created_at { get; set; } = DateTime.UtcNow;

        public DateTime? updated_at { get; set; }



    }
}
