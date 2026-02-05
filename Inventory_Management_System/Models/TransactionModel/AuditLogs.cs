namespace Inventory_Management_System.Models
{
    public class AuditLogs
    {
        public int Id { get; set; }

        public Guid? UserId { get; set; }

        public string Action { get; set; } = null!;

        public string EntityName { get; set; } = null!;

        public int EntityId { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
