namespace Inventory_Management_System.Models.StoreModels
{
    public class User
    {

        public int id { get; set; }

        public string first_name { get; set; } = null!;

        public string last_name { get; set; } = null!;

        public string username { get; set; } = null!;

        public string email { get; set; } = null!;

        public string passwordhash { get; set; } = null!;

        public string role { get; set; } = "User";

        public int theme_id { get; set; } = 1;

        public string? ProfileImagePath { get; set; }
    }
}
