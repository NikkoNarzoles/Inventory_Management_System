using Inventory_Management_System.Models;
using Inventory_Management_System.Repositories.Interfaces;
using Inventory_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_System.Controllers
{

    [Authorize(Roles = "Admin")]
    public class PurchaseController : Controller
    {

        private readonly IPurchaseService _purchaseService;
        private readonly IUserRepository _userRepository;

        public PurchaseController(IPurchaseService purchaseService,
                                   IUserRepository userRepository)
        {
              _purchaseService = purchaseService;

              _userRepository = userRepository;   

        }


        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var purchases = await _purchaseService.ListPurchasesAsync();
            var users = await _userRepository.GetAllAsync();

            var vm = _purchaseService.MapToPurchaseViewModels(purchases, users);

            return View(vm);
        }



        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================

        [HttpGet]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var purchase = await _purchaseService.GetPurchaseAsync(id.Value);

            if (purchase == null)
                return NotFound();

            var vm = _purchaseService.MapToPurchaseViewModel(purchase);

            return View(vm);
        }




        //=================================================================================================================


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _purchaseService.DeletePurchaseAsync(id);
            return RedirectToAction(nameof(Index));
        }




        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================












    }
}
