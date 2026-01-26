using Inventory_Management_System.Services.ServiceInterface;
using Inventory_Management_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Inventory_Management_System.Controllers
{
    public class UserController : Controller
    {




        private readonly IUserService _IuserService;

       
        public UserController(IUserService userService)
        {
            _IuserService = userService;
        }

        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }



        // GET: Users
        public async Task<IActionResult> Index()
        {
            var items = await _IuserService.ShowAsync();

            return View(items);
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var dto = await _IuserService.EditAsync(id.Value);

            if (dto == null)
                return NotFound();

            var vm = new EditUserViewModel
            {
                id = dto.id,
                first_name = dto.first_name,
                last_name = dto.last_name,
                username = dto.username,
                email = "" // or fetch email via entity if needed
            };

            return View(vm);
        }

        //=================================================================================================================
        //=================================================================================================================



        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var success = await _IuserService.UpdateAsync(vm);
            if (!success)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }



    }
}
