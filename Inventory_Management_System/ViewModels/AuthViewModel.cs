using System.ComponentModel.DataAnnotations;

namespace Inventory_Management_System.ViewModels
{
    public class AuthViewModel
    {
        [Required]
        [Display(Name = "Username or Email")]
        public string UsernameOrEmail { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

}
