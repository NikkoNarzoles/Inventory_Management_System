using Inventory_Management_System.Repositories.Interfaces;
using Inventory_Management_System.Services.ServiceInterface;
using Inventory_Management_System.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace Inventory_Management_System.Controllers
{
    public class UserController : Controller
    {




        private readonly IUserService _IuserService;

        private readonly IUserRepository _IuserRepo;

        private readonly IProfileRepository _IprofileRepo;

       
        public UserController(IUserService userService, IUserRepository userReposy, IProfileRepository profileRepository)
        {
            _IuserService = userService;

            _IuserRepo = userReposy;

            _IprofileRepo = profileRepository;
        }





        [Authorize]
        public async Task<IActionResult> Profile(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            var user = await _IuserRepo.GetByIdAsync(id.Value);
            if (user == null)
                return NotFound();

            var ownItems = await _IprofileRepo.GetOwnItems(id.Value);

            var vm = new UserProfileViewModel
            {
                User = user,
                Items = ownItems.ToList()
            };

            return View(vm);
        }












        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }


        [Authorize]
        // GET: Users
        public async Task<IActionResult> Index(string? search)
        {

            if (string.IsNullOrWhiteSpace(search))
            {
                var users = await _IuserService.ShowAsync();

                ViewData["Search"] = null;
                ViewData["UserNotFound"] = false;

                return View(users);
            }

            var filteredUsers = await _IuserRepo.SearchAsync(search);

            ViewData["Search"] = search;

            if (!filteredUsers.Any())
            {
                ViewData["UserNotFound"] = true;
                return View(filteredUsers); 
            }

            ViewData["UserNotFound"] = false;
            return View(filteredUsers);
        }






        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================



        // GET: User/Register
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                TempData["Error"] = "You are already logged in. Please log out first before registering another account.";
                return RedirectToAction("Logout", "Auth");
            }

            return View();
        }



        //=================================================================================================================
        //=================================================================================================================


        // POST: User/Register

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            bool exists = await _IuserRepo.UserNameVerification(viewModel.username);

            if (exists)
            {
                ModelState.AddModelError(nameof(viewModel.username), "Username already exists");
                return View(viewModel);
            }


            await _IuserService.RegisterAsync(viewModel);

            return RedirectToAction("Index", "Home");
        }





        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================





        //get
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete (int? id)
        {
            if (id == null) {
                return NotFound();   
            }

            var items = await _IuserService.GetDeleteAsync(id.Value);

            if (items == null) {
                return NotFound();
               }

            return View(items);

        }


        //=================================================================================================================
        //=================================================================================================================


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _IuserService.DeleteAsync(id);

            if (!success)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }



        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================

        //get 
        [Authorize]
        public async Task<IActionResult> Edit(int id, string? returnUrl)
        {
            var dto = await _IuserService.EditAsync(id);
            if (dto == null)
                return NotFound();

            var vm = new EditUserViewModel
            {
                id = dto.id,
                first_name = dto.first_name,
                last_name = dto.last_name,
                username = dto.username,
                theme_id = dto.theme_id,
                ReturnUrl = returnUrl
            };

            return View(vm);
        }

        //=================================================================================================================
        //=================================================================================================================



        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var success = await _IuserService.UpdateAsync(vm);
            if (!success)
                return NotFound();

            if (!string.IsNullOrEmpty(vm.ReturnUrl) && Url.IsLocalUrl(vm.ReturnUrl))
                return Redirect(vm.ReturnUrl);

            return RedirectToAction(nameof(Index));
        }



        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================


        [Authorize]
        public async Task<IActionResult> DeleteAccount()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var user = await _IuserService.GetDeleteAsync(userId);

            if (user == null)
                return NotFound();

            return View(user);

        }




        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccountConfirmed()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var success = await _IuserRepo.DeleteprofileAsync(userId);

            if (!success)
                return NotFound();

            await HttpContext.SignOutAsync();

            return RedirectToAction("Login", "Auth");
        }












    }
}
