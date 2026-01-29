using System.ComponentModel.DataAnnotations;

namespace Inventory_Management_System.ViewModels
{
    public class EditUserViewModel
    {
        public int id { get; set; }

        [Required]
        public string first_name { get; set; } = null!;

        [Required]
        public string last_name { get; set; } = null!;

        [Required]
        public string username { get; set; } = null!;


        public string? ReturnUrl { get; set; }


        [Range (1,4)]
        public int theme_id { get; set; }
    }

}
