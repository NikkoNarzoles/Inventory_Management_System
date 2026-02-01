using Inventory_Management_System.DTOs;
using Inventory_Management_System.Repositories.Interfaces;
using Inventory_Management_System.Services.Interfaces;
using Inventory_Management_System.Services.ServiceInterface;
using Inventory_Management_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;



namespace Inventory_Management_System.Controllers
{
    public class StoreItemsController : Controller
    {

        private readonly IStoreItemsRepository _Irepository;

        private readonly IStoreItemsService _Iservice;

        private readonly IPurchaseService _purchaseService;

          
        public StoreItemsController(IStoreItemsRepository storeItemsRepository,
                                    IStoreItemsService SIS,
                                    IPurchaseService purchaseService)
        {
            _Irepository = storeItemsRepository;

            _Iservice = SIS;

            _purchaseService = purchaseService;
   
        }




        [Authorize]
        public async Task<IActionResult> Buy(int id, string? returnUrl)
        {
            var vm = await _Iservice.Buymap(id, 1);
            vm.ReturnUrl = returnUrl;
            return View(vm);
        }


        [Authorize]
        public async Task<IActionResult> PreBuyConfirm(int id, int quan, string? returnUrl)
        {
            var vm = await _Iservice.Buymap(id, quan);
            vm.ReturnUrl = returnUrl;
            return View(vm);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyConfirm(int id, int quan, string? returnUrl)
        {
            try
            {


                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

               
                var purchaseId = await _Iservice.BuyConfirmAsync(id, quan, userId);

                return RedirectToAction(nameof(BuySuccess),new { purchaseId, returnUrl });
            }
            catch (Exception ex)
            {
                
                TempData["Error"] = ex.Message;


                return RedirectToAction(nameof(PreBuyConfirm),new { id, quan, returnUrl });
            }
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> BuySuccess(int purchaseId, string? returnUrl)
        {
            var purchase = await _purchaseService.GetPurchaseAsync(purchaseId);

            if (purchase == null)
            {
                return NotFound();
            }

            var vm = _purchaseService.MapToBuyViewModel(purchase);
            vm.ReturnUrl = returnUrl;


            return View(vm);
        }






        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }

        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================


        [Authorize]
        // GET: StoreItems
        public async Task<IActionResult> Index(string? search, string? mode, StoreItemSortBy? sortBy )
        {
           
            if (mode == "users")
            {            
                return RedirectToAction("Index","User", new { search = search });
            }

            var items = await _Iservice.GetItemsAsync(search, sortBy);

            ViewData["Search"] = search;
            ViewData["SortBy"] = sortBy;
            ViewData["Mode"] = "items";

            return View(items);
        }








        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================


        [Authorize]
        // GET: StoreItems/Create
        public IActionResult Create(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        //=================================================================================================================
        //=================================================================================================================

        // POST: StoreItems/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StoreItemsViewModels viewModel, string? returnUrl)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _Iservice.CreateAsync(viewModel, userId);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Index));
        }






        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================



        [Authorize(Roles = "Admin")]
        // GET: StoreItems/Edit/5
        public async Task<IActionResult> Edit(int? id)

        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _Irepository.GetByIdAsync(id.Value);

            if (item == null)
            {
                return NotFound();
            }

            var vm = await _Iservice.EditMapAsync(id.Value);


            return View(vm);
        }



        //=================================================================================================================
        //=================================================================================================================



        [Authorize(Roles = "Admin")]
        // POST: StoreItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StoreItemsViewModels viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var success = await _Iservice.EditAsync(viewModel, id);

            if (!success)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }


        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================



        [Authorize(Roles = "Admin")]
        // GET: StoreItems/Delete/5
        public async Task<IActionResult> Delete (int? id) //parameters
        {

            if (id == null) 
            {
                return NotFound();
            }


            var item = await _Irepository.GetByIdAsync(id.Value);



            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }


        //=================================================================================================================
        //=================================================================================================================


        [Authorize(Roles = "Admin")]
        // POST: StoreItems/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            await  _Irepository.DeleteAsync(id);
     
            return RedirectToAction(nameof(Index));
        }



        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================
        



    }
}
