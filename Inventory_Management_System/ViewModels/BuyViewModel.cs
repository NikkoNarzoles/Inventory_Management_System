namespace Inventory_Management_System.ViewModels
{
    public class BuyViewModel
    {
     
        public int id { get; set; }
        public string item_name { get; set; } = null!;
        public string? description { get; set; }
        public int quan { get; set; } =  1;
        public decimal price { get; set; }
        public decimal total_price { get; set; }
        public DateTime purchase_date { get; set; }

    }
}
