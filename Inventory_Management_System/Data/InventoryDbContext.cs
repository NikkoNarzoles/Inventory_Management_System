using Inventory_Management_System.Models;
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

            base.OnModelCreating(modelBuilder);
        }




    }
}
