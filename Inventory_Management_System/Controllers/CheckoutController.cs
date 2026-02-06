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
    private readonly IWalletService _walletService;



    public CheckoutController(ICheckoutService checkoutService,
                              ICartService cartService,
                              IOrderRepository iorderRepo,
                              IWalletService walletService)
    {
        _checkoutService = checkoutService;
        _cartService = cartService;
        _IorderRepo = iorderRepo;
        _walletService = walletService;
    }

    // ⭐ NEW — Confirmation Page
    public async Task<IActionResult> Index()
    {
        int userId = GetUserId();

        ViewBag.WalletBalance = await _walletService.GetUserWalletBalanceAsync(userId, "User");

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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckoutSelected(string paymentMethod)
    {
        try
        {
            int userId = GetUserId();

            ViewBag.WalletBalance = await _walletService.GetUserWalletBalanceAsync(userId, "User");

            int? orderId;

            if (paymentMethod == "Credit")
                orderId = await _checkoutService.CheckoutSelectedItemsCredit(userId);
            else
                orderId = await _checkoutService.CheckoutSelectedItems(userId);

            if (orderId == null)
                return RedirectToAction("Index", "Cart");

            return RedirectToAction("Receipt", new { id = orderId });
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index", "Cart");
        }
    }











    // ⭐ Step 2 — Receipt Page
    public async Task<IActionResult> Receipt(int id)
    {

        int userId = GetUserId();

        ViewBag.WalletBalance = await _walletService.GetUserWalletBalanceAsync(userId, "User");

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
