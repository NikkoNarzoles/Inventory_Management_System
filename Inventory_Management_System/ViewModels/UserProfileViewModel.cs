using Inventory_Management_System.DTOs;

namespace Inventory_Management_System.ViewModels
{
    public class UserProfileViewModel
    {
        public UserDto User { get; set; } = null!;
        public List<StoreItemsDto> Items { get; set; } = new();
    }
}