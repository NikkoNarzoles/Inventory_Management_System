using Inventory_Management_System.Services.ServiceInterface;
using Inventory_Management_System.Repositories.Interfaces;  
using Inventory_Management_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
public class CheckoutController : Controller
{
    private readonly ICheckoutService _checkoutService;
    private readonly ICartService _cartService;
    private readonly IOrderRepository _IorderRepo;
   



    public CheckoutController(ICheckoutService checkoutService,
                              ICartService cartService,
                              IOrderRepository iorderRepo)
    {
        _checkoutService = checkoutService;
        _cartService = cartService;
        _IorderRepo = iorderRepo;
    }

    // ⭐ NEW — Confirmation Page
    public async Task<IActionResult> Index()
    {
        int userId = GetUserId();

        var cart = await _cartService.GetActiveCart(userId);

        if (cart == null)
            return RedirectToAction("Index", "Cart");

        var vm = new CheckoutIndexViewModel();

        foreach (var item in cart.CartItems.Where(x => x.IsSelected))
        {
            vm.Items.Add(new CheckoutItemVM
            {
                CartItemId = item.Id,
                ItemName = item.StoreItem.item_name,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                InputQuantity = item.Quantity
            });

            vm.TotalAmount += item.Quantity * item.UnitPrice;
        }

        return View(vm);
    }




    // ⭐ Step 1 — Confirm Checkout
    [HttpPost]
    public async Task<IActionResult> CheckoutSelected()
    {
        int userId = GetUserId();

        var orderId = await _checkoutService.CheckoutSelectedItems(userId);

        if (orderId == null)
            return RedirectToAction("Index", "Cart");

        return RedirectToAction("Receipt", new { id = orderId });
    }








    // ⭐ Step 2 — Receipt Page
    public async Task<IActionResult> Receipt(int id)
    {
        var order = await _IorderRepo.GetOrderWithItems(id);

        if (order == null)
            return RedirectToAction("Index", "Cart");

        return View(order);
    }






    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
