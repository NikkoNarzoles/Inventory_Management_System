using Inventory_Management_System.DTOs;
using Inventory_Management_System.Models;
using Inventory_Management_System.Repositories.Interfaces;
using Inventory_Management_System.Services.Interfaces;
using Inventory_Management_System.Services.ServiceImplementation;
using Inventory_Management_System.Services.ServiceInterface;
using Inventory_Management_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
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
        public async Task<IActionResult> Buy(int id)
        {
            var vm = await _Iservice.Buymap(id, 1); 
            return View(vm);
        }


        [Authorize]
        public async Task<IActionResult> PreBuyConfirm(int id, int quan)
        {
            var vm = await _Iservice.Buymap(id, quan);
            return View(vm);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyConfirm(int id, int quan)
        {
            try
            {


                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

               
                var purchaseId = await _Iservice.BuyConfirmAsync(id, quan, userId);

                return RedirectToAction(nameof(BuySuccess), new { purchaseId });
            }
            catch (Exception ex)
            {
                
                TempData["Error"] = ex.Message;

               
                return RedirectToAction(nameof(PreBuyConfirm), new { id, quan });
            }
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> BuySuccess(int purchaseId)
        {
            var purchase = await _purchaseService.GetPurchaseAsync(purchaseId);

            if (purchase == null)
            {
                return NotFound();
            }

            var vm = _purchaseService.MapToBuyViewModel(purchase);

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
        public async Task<IActionResult> Index(string? search,StoreItemSortBy? sortBy )
        {
            var items = await _Iservice.GetItemsAsync(search, sortBy);

            ViewData["Search"] = search;
            ViewData["SortBy"] = sortBy;

            return View(items);
        }







        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================


        [Authorize(Roles = "Admin")]
        // GET: StoreItems/Create
        public IActionResult Create ()
        {
            return View();

        }


        //=================================================================================================================
        //=================================================================================================================

        [Authorize(Roles = "Admin")]
        // POST: StoreItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create  (StoreItemsViewModels viewModel)
        {
                if (!ModelState.IsValid)
                {   
                    return View(viewModel);
                }

                var item = new StoreItem
                {
                    item_code = viewModel.item_code,
                    item_name = viewModel.item_name,
                    description = viewModel.description,
                    quantity = viewModel.quantity,
                    price = viewModel.price,
                    supplier = viewModel.supplier,
                    created_at = DateTime.UtcNow
                };

            await _Irepository.AddAsync(item);

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

            var viewModel = 
                new StoreItemsDto
                                        {
                                            id = item.id,
                                            item_code = item.item_code!,
                                            item_name = item.item_name!,
                                            description = item.description,
                                            quantity = item.quantity,
                                            price = item.price
                                        };

            return View(viewModel);
        }



        //=================================================================================================================
        //=================================================================================================================



        [Authorize(Roles = "Admin")]
        // POST: StoreItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StoreItemsViewModels viewModel)
        {
            if (id != viewModel.id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var item = await _Irepository.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            item.item_code = viewModel.item_code;
            item.item_name = viewModel.item_name;
            item.description = viewModel.description;
            item.quantity = viewModel.quantity;
            item.price = viewModel.price;
            item.supplier = viewModel.supplier;
            item.updated_at = DateTime.UtcNow;

            await _Irepository.UpdateAsync(item);

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
