
using Inventory_Management_System.Data;
using Inventory_Management_System.Services.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


namespace Inventory_Management_System.Controllers
{
    public class HomeController : Controller
    {

        private readonly IWalletService _walletService;

        public HomeController(IWalletService walletService)
        {
            _walletService = walletService;
        }


        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                // ? already logged in
                return RedirectToAction("Index", "StoreItems");
            }

            // ? not logged in
            return RedirectToAction("Login", "Auth");





        }




        [Authorize(Roles = "Admin")]
        [HttpGet("seed-admin")]
        public async Task<IActionResult> SeedAdmin()
        {
            await _walletService.SeedAdminBalanceAsync(1003, 100000000m);
            return Ok("Admin balance seeded");
        }













    }
}
