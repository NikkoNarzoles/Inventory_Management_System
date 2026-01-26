namespace Inventory_Management_System.DTOs
{
    public class StoreItemsDto
    {
        public int id { get; set; }
        public string item_code { get; set; } =  null!;
        public required string item_name { get; set; } = null!;
        public string? description { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
    }
}
