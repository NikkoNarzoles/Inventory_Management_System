using Inventory_Management_System.Models.StoreModels;
using System.Security.Claims;

namespace Inventory_Management_System.Services.ServiceInterface
{
    public interface IAuthService
    {

        Task<User?> VerifyUserAsync(string usernameOrEmail, string password);

        ClaimsPrincipal CreatePrincipal(User user);

    }
}
