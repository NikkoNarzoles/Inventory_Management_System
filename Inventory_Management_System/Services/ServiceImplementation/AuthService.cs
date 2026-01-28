using Inventory_Management_System.Models;
using Inventory_Management_System.Repositories.Interfaces;
using Inventory_Management_System.Services.ServiceInterface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace Inventory_Management_System.Services.ServicesImplementation
{
    public class AuthService : IAuthService


    {
        private readonly IUserRepository _IuserRepository;

        public AuthService(IUserRepository authRepository)
        {
            _IuserRepository = authRepository;
        }




      //=================================================================================================================
      //=================================================================================================================



        public async Task<User?> VerifyUserAsync(string usernameOrEmail, string password)
        {
            var user = await _IuserRepository
                .GetByUsernameOrEmailAsync(usernameOrEmail);

            if (user == null)
                return null;

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(
                password,
                user.passwordhash
            );



            return isValidPassword ? user : null;
        }

        //=================================================================================================================
        //=================================================================================================================



        // 🔐 BUSINESS LOGIC: build claims
        public ClaimsPrincipal CreatePrincipal(User user)
        {
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                        new Claim(ClaimTypes.Name, user.username),
                        new Claim(ClaimTypes.Role, user.role),
                        new Claim(ClaimTypes.GivenName, user.first_name)

                    };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            return new ClaimsPrincipal(identity);
        }




    }
}