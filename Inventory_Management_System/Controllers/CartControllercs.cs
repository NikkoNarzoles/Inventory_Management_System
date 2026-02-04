using Inventory_Management_System.Services.ServiceInterface;
using Inventory_Management_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }


    [Authorize]
    public async Task<IActionResult> Index()
    {
        int userId = GetUserId();

        var cart = await _cartService.GetActiveCart(userId);

        var vm = new CartViewModel();

        if (cart?.CartItems != null)
        {
            foreach (var item in cart.CartItems)
            {
                vm.Items.Add(new CartItemVM
                {
                    CartItemId = item.Id,
                    ItemName = item.StoreItem.item_name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    IsSelected = item.IsSelected
                });
            }
        }

        return View(vm);
    }


    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToCart(int storeItemId)
    {
        int userId = GetUserId();

        await _cartService.AddToCart(userId, storeItemId);

        return RedirectToAction("Index");
    }


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
    {
        await _cartService.UpdateQuantity(cartItemId, quantity);

        return RedirectToAction("Index");
    }



    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ToggleSelect(int cartItemId, bool selected)
    {
        await _cartService.ToggleSelect(cartItemId, selected);

        return RedirectToAction("Index"); 
    }



    [Authorize]
    [HttpPost]
    public async Task<IActionResult> RemoveItem(int cartItemId)
    {
        await _cartService.RemoveItem(cartItemId);

        return RedirectToAction("Index");
    }

    [Authorize]
    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
