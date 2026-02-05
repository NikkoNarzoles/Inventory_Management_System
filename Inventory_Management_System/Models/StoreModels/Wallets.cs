using Inventory_Management_System.Models.TransactionModel;

namespace Inventory_Management_System.Models.StoreModels
{
    public class Wallets
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string WalletType { get; set; } = "User";   

        public string Status { get; set; } = "Active";    

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User? User { get; set; }

        public ICollection<LedgerEntries>? LedgerEntries { get; set; }

        

    }
}
