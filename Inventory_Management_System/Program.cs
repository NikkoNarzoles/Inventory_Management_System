using Inventory_Management_System.Data;
using Inventory_Management_System.Repositories.Implementations;
using Inventory_Management_System.Repositories.Interfaces;
using Inventory_Management_System.Services.Implementations;
using Inventory_Management_System.Services.Interfaces;
using Inventory_Management_System.Services.ServiceImplementation;
using Inventory_Management_System.Services.ServiceInterface;
using Inventory_Management_System.Services.ServicesImplementation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InventoryDbContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";

        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();



builder.Services.AddScoped< IUserRepository, UserRepository>();

builder.Services.AddScoped< IStoreItemsRepository, StoreItemsRepository>();

builder.Services.AddScoped< IPurchaseRepository, PurchaseRepository>();

builder.Services.AddScoped< IUserService, UserService>();

builder.Services.AddScoped< IAuthService, AuthService>();

builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddScoped<ICheckoutService, CheckoutService>();

builder.Services.AddScoped< IWalletService, WalletService>();



builder.Services.AddScoped< IStoreItemsService, StoreItemsService>();

builder.Services.AddScoped< IPurchaseService, PurchaseService>();

builder.Services.AddScoped< IProfileRepository, ProfileRepository>();

builder.Services.AddScoped<ICartRepository, CartRepository>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();




// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
                       .GetRequiredService<InventoryDbContext>();

    DataInitializer.Seed(context);
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();