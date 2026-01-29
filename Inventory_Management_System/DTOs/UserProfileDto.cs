namespace Inventory_Management_System.DTOs
{
    public class UserProfileDto
    {
        public int id { get; set; }
        public int owners_id { get; set; }
        public required string item_name { get; set; } = null!;
        public string? description { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }




        public string first_name { get; set; } = null!;

        public int theme_id { get; set; }

    }
}
