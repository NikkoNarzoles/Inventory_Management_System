namespace Inventory_Management_System.Models.TransactionModel
{
    public class Transactions
    {
        public int Id { get; set; }

        public string ReferenceNo { get; set; } = null!;   // ORDER123, CASHIN001

        public string Type { get; set; } = null!;          // Payment, CashIn, Refund, Adjustment

        public string Status { get; set; } = "Pending";    // Pending, Completed, Failed

        public decimal Amount { get; set; }

        public Guid CreatedByUserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }

        // Navigation
        public ICollection<LedgerEntries>? LedgerEntries { get; set; }
    }
}
