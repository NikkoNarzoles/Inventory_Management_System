using Inventory_Management_System.DTOs;
using Inventory_Management_System.ViewModels;

namespace Inventory_Management_System.Services.ServiceInterface
{
    public interface IUserService
    {
       Task<IEnumerable<UserDto>> ShowAsync();

       Task RegisterAsync(UserViewModel viewModel);

        Task<UserDto?> GetDeleteAsync(int id);

        Task<bool> DeleteAsync(int id);
        
        Task<UserDto?> EditAsync(int id);

        Task<bool> UpdateAsync(EditUserViewModel vm);

       
    }
}
