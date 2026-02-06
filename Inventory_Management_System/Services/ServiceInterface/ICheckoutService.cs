using Inventory_Management_System.Models;

namespace Inventory_Management_System.Services.ServiceInterface
{
    public interface ICheckoutService
    {
        Task<int?> CheckoutSelectedItems(int userId);

        Task<int?> CheckoutSelectedItemsCredit(int userId);

    }

}
