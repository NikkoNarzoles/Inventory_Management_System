using Inventory_Management_System.Models;
using Inventory_Management_System.Repositories.Interfaces;
using Inventory_Management_System.Services.ServiceInterface;
using Inventory_Management_System.Services.ServicesImplementation;
using Inventory_Management_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace Inventory_Management_System.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IProfileRepository _profileRepo;

        private readonly IStoreItemsRepository _storeRepo;

        private readonly IStoreItemsService _storeService;

        private readonly IUserRepository _userRepository;

        private readonly IUserService _IuserService;


        public ProfileController(IProfileRepository profileRepository, IStoreItemsRepository storeItemsRepository, 
                                 IStoreItemsService storeService,  IUserRepository  userRepository, IUserService userService)
        {
            _profileRepo = profileRepository;

            _storeRepo = storeItemsRepository;

            _storeService = storeService;

            _userRepository = userRepository;

            _IuserService = userService;
        }



        [Authorize]
        public async Task <IActionResult> Index()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);


            var items =  await _profileRepo.GetOwnItems(userId);

            ViewBag.ThemeId = await _profileRepo.GetUserThemeId(userId);

            return View(items);
        }


        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _storeRepo.GetByIdAsync(id.Value);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _storeRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }





        [Authorize]
        public async Task<IActionResult> Edit(int? id) 
        {
            if (id == null)
                return NotFound();

            var vm = await _storeService.EditMapAsync(id.Value);

            if (vm == null)
                return NotFound();

            return View(vm);
        }



        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StoreItemsViewModels viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var success = await _storeService.EditAsync(viewModel, id);

            if (!success)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }




        [Authorize]
        public async Task<IActionResult> EditProfile(int? id, string? returnUrl = null)
        {
            int userId = id ?? int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var dto = await _IuserService.EditAsync(userId);
            if (dto == null)
                return NotFound();

            return RedirectToAction("Edit","User",new {id = dto.id, returnUrl = returnUrl?? Request.Headers["Referer"].ToString()}
           );
        }




    }
}
