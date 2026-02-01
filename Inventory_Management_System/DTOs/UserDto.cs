namespace Inventory_Management_System.DTOs
{
    public class UserDto
    {
        public int id { get; set; }
        public string first_name { get; set; } = null!;

        public string last_name { get; set; } = null!;

        public string username { get; set; } = null!;

        public int theme_id { get; set; }

        public int ItemCount { get; set; }
    }
}
