using Inventory_Management_System.Repositories.Interfaces;
using Inventory_Management_System.Services.ServiceInterface;
using Inventory_Management_System.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace Inventory_Management_System.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IProfileRepository _profileRepo;

        private readonly IStoreItemsRepository _storeRepo;

        private readonly IStoreItemsService _storeService;

     

        public ProfileController(IProfileRepository profileRepository, IStoreItemsRepository storeItemsRepository, 
                                 IStoreItemsService storeService)
        {
            _profileRepo = profileRepository;

            _storeRepo = storeItemsRepository;

            _storeService = storeService;

           
        }




          public async Task <IActionResult> Index()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var items =  await _profileRepo.GetOwnItems(userId);


            return View(items);
        }



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



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _storeRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }






        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var vm = await _storeService.EditMapAsync(id.Value);

            if (vm == null)
                return NotFound();

            return View(vm);
        }


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




    }
}
