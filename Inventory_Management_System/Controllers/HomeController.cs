
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Inventory_Management_System.Controllers
{
    public class HomeController : Controller
    {
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
    }
}
