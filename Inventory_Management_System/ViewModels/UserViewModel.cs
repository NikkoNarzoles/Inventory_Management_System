using System.ComponentModel.DataAnnotations;


namespace Inventory_Management_System.ViewModels
{
    public class UserViewModel
    {
        public int id { get; set; }

        [Required(ErrorMessage = "First Name is requied")]
        [StringLength(20, ErrorMessage = "First Name is too long")]
        public string first_name { get; set; } = null!;


        [Required(ErrorMessage = "Last Name is requied")]
        [StringLength(20, ErrorMessage = "Last Name is too long")]
        public string last_name { get; set; } = null!;


        [Required(ErrorMessage = "Username is requied")]
        [StringLength(15, ErrorMessage = "Username is too long")]
        public string username { get; set; } = null!;

        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage ="Enter valid Email")]
        [MaxLength(50)]
        public string email { get; set; } = null!;


        [Required(ErrorMessage ="Enter Password")]
        public string passwordhash { get; set; } = null!;


        


    }
}


