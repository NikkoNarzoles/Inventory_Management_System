using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory_Management_System.ViewModels
{
    public class StoreItemsViewModels
    {

        public int id { get; set; }


        public int owners_id { get; set; }


        [Required (ErrorMessage ="Item Id is required")]
        [StringLength(20, ErrorMessage ="Exceeds 20 letters")] 
        public required string item_code { get; set; } 


        [Required(ErrorMessage ="Item Name is required")]
        [StringLength(30, ErrorMessage ="Exceeds 30 letters")]
        public required string item_name { get; set; } 


        [StringLength(50, ErrorMessage = "Exceeds 50 letters")]
        public string? description { get; set; }




        [Required(ErrorMessage = "Quantity is required")]
        [Range(1,99999999, ErrorMessage = "Set Right Quantity")]
        public int quantity { get; set; } 




        [Required(ErrorMessage = "Price must be set")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(typeof(decimal), "0.01", "99999999999", ErrorMessage = "Set Right Price")]

        public decimal price { get; set; }





        public string? supplier { get; set; }

        public DateTime created_at { get; set; } = DateTime.UtcNow;
        public DateTime? updated_at { get; set; }
    }
}
