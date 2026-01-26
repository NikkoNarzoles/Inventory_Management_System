using Inventory_Management_System.Models;
using System.Security.Claims;

namespace Inventory_Management_System.Services.ServiceInterface
{
    public interface IAuthService
    {

        Task<User?> VerifyUserAsync(string usernameOrEmail, string password);

        ClaimsPrincipal CreatePrincipal(User user);

    }
}
