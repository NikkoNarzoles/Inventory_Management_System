using Inventory_Management_System.Data;
using Inventory_Management_System.Models.StoreModels;
using Microsoft.EntityFrameworkCore;


namespace Inventory_Management_System.Data { 
            public static class DataInitializer
            {
                public static void Seed(InventoryDbContext context)
                {
                    // Apply pending migrations automatically
                    context.Database.Migrate();

                    // =========================
                    // SEED ADMIN USER
                    // =========================

                    if (!context.Users.Any(u => u.email == "admin@system.com"))
                    {
                        var adminUser = new User
                        {
                            first_name = "System",
                            last_name = "Admin",
                            username = "Admin",            
                            email = "admin@system.com",
                            passwordhash = BCrypt.Net.BCrypt.HashPassword("1qa2ws3ed"),
                            role = "Admin"
                        };

                       context.Users.Add(adminUser);
                        context.SaveChanges();
                    }


                        context.SaveChanges();
                    }
                }
}
