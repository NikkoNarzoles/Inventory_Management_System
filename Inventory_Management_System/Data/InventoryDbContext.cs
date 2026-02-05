using Inventory_Management_System.Models;
using Inventory_Management_System.Models.StoreModels;
using Inventory_Management_System.Models.TransactionModel;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management_System.Data
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
             : base(options)
        { }

        public DbSet<StoreItem> StoreItems { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Wallets> Wallets { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<LedgerEntries> LedgerEntries { get; set; }
        public DbSet<AuditLogs> AuditLogs { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StoreItem>()
                .Property(x => x.price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Purchase>()
                .Property(x => x.price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Purchase>()
                .Property(x => x.total_price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<CartItem>()
                .Property(p => p.UnitPrice)
                .HasPrecision(18, 2);

            
            modelBuilder.Entity<Order>()
                .Property(p => p.TotalAmount)
                .HasPrecision(18, 2);

        
            modelBuilder.Entity<OrderItem>()
                .Property(p => p.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LedgerEntries>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transactions>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);


            base.OnModelCreating(modelBuilder);
        }




    }
}
