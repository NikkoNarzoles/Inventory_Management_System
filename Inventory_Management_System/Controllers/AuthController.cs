using Inventory_Management_System.DTOs;
using Inventory_Management_System.Services.ServiceInterface;
using Inventory_Management_System.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Inventory_Management_System.Controllers
{
    public class AuthController : Controller
    {

        public readonly IAuthService _IauthService;


        public AuthController(IAuthService authService)
        {
            _IauthService = authService;
        }


        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================



        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }

        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================



        [Authorize]
        public IActionResult Index()
        {
            var dto = new UserDto
            {
                first_name = TempData["FirstName"]?.ToString() ?? "",
                last_name = TempData["LastName"]?.ToString() ?? ""
            };

            return View(dto);
        }


        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================



        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        //=================================================================================================================
        //=================================================================================================================



        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AuthViewModel viewModel)
        {

            // BLOCK if already logged in
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError(
                    "",
                    "You are already logged in. Please log out first before logging in to another account."
                );
                return View(viewModel);
            }



            if (!ModelState.IsValid)
                return View(viewModel);


            var user = await _IauthService.VerifyUserAsync(viewModel.UsernameOrEmail, viewModel.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(viewModel);
            }

            var principal = _IauthService.CreatePrincipal(user);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            return RedirectToAction("Index", "StoreItems");
        }




        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================




        [Authorize]
        public async Task<IActionResult> Logout()
        {
            return View();
        }




        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutConfirm()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );
            return RedirectToAction("Login", "Auth");

        }


     





    }

}
