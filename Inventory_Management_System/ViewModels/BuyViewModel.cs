using System.ComponentModel.DataAnnotations;

namespace Inventory_Management_System.ViewModels
{
    public class BuyViewModel
    {
     
        public int id { get; set; }
        public string item_name { get; set; } = null!;
        public string? description { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "99999999999", ErrorMessage = "Set Right Price")]
        public int quan { get; set; } =  1;


        [Required]
        [Range(typeof(decimal), "0.01", "99999999999", ErrorMessage = "Set Right Price")]
        public decimal price { get; set; }

        public decimal total_price { get; set; }
        public DateTime purchase_date { get; set; }

        public string? ReturnUrl { get; set; } 

    }
}
