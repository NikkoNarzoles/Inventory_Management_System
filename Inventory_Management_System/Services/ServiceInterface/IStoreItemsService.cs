using Inventory_Management_System.DTOs;
using Inventory_Management_System.ViewModels;

namespace Inventory_Management_System.Services.ServiceInterface
{
    public interface IStoreItemsService
    {

        Task<BuyViewModel> Buymap(int id, int quan);

        Task<int> BuyConfirmAsync(int id, int quan, int userId);

        Task<List<StoreItemsDto>> GetItemsAsync(string? search, StoreItemSortBy? sortBy);

        Task CreateAsync(StoreItemsViewModels viewModel, int userId);

        Task<StoreItemsViewModels> EditMapAsync(int id);

        Task<bool> EditAsync(StoreItemsViewModels viewModel, int id);

    }
}
