using Humanizer;
using Inventory_Management_System.DTOs;
using Inventory_Management_System.Models;
using Inventory_Management_System.Repositories.Interfaces;
using Inventory_Management_System.Services.ServiceInterface;
using Inventory_Management_System.ViewModels;
using Microsoft.EntityFrameworkCore;



namespace Inventory_Management_System.Services.ServicesImplementation
{
    public class UserService : IUserService
    {


        private readonly IUserRepository _Irepository;

        private readonly IProfileRepository _profileRepository;

        private readonly IWebHostEnvironment _env;

        public UserService (IUserRepository UserRepository, IProfileRepository profileRepository, IWebHostEnvironment env)
        {
            _Irepository = UserRepository;

            _profileRepository = profileRepository;

            _env = env;
        }


        //=================================================================================================================
        //=================================================================================================================

        //read
        public async Task<IEnumerable<UserDto>> ShowAsync()
        {
           return await _Irepository.GetAllAsync();
        }



        //=================================================================================================================
        //=================================================================================================================

        //create
        public async Task RegisterAsync(UserViewModel viewModel)
        {
            // 0. Check if an Admin already exists
            bool adminExists = await _Irepository.AnyAsync(u => u.role == "Admin");

            // 1. Hash the password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(viewModel.passwordhash);

            // 2. Map ViewModel → Model
            var user = new User
            {
                first_name = viewModel.first_name,
                last_name = viewModel.last_name,
                username = viewModel.username,
                email = viewModel.email,
                passwordhash = hashedPassword,
                theme_id = 1,

                // FIRST USER BECOMES ADMIN
                role = adminExists ? "User" : "Admin"
            };

            // 3. Save to database
            await _Irepository.AddAsync(user);
        }


        //=================================================================================================================
        //=================================================================================================================


        //get delete
        public async Task<UserDto?> GetDeleteAsync(int id)
        {
            return await _Irepository.GetByIdAsync(id);
        }

        //post delete
        public async Task<bool> DeleteAsync(int id)
        {
            return await _Irepository.DeleteAsync(id);
        }


        //=================================================================================================================
        //=================================================================================================================


        //get
        public async Task<UserDto?> EditAsync(int id)
        {
            return await _Irepository.GetByIdAsync(id);
        }

        // UPDATE
        public async Task<bool> UpdateAsync(EditUserViewModel vm)
        {
            var user = await _Irepository.FindAsync(vm.id);
            if (user == null)
                return false;

            user.first_name = vm.first_name;
            user.last_name = vm.last_name;
            user.username = vm.username;
            
            user.theme_id = vm.theme_id;

            await _Irepository.UpdateAsync(user);
            return true;
        }


        public EditUserViewModel Imthemap (UserDto dto, string returnUrl)
        {
            var vm = new EditUserViewModel
            {
                id = dto.id,
                first_name = dto.first_name,
                last_name = dto.last_name,
                username = dto.username,
                theme_id = dto.theme_id,
                ReturnUrl = returnUrl
            };

            return (vm);
        }


        public async Task<bool> DeleteProfileImage(int userId)
        {
            var user = await _Irepository.FindAsync(userId);
            if (user == null) return false;

            // If already default or null, nothing to delete
            if (string.IsNullOrEmpty(user.ProfileImagePath))
                return true;

            // NEVER delete default image
            if (user.ProfileImagePath.Contains("Default_pfp"))
                return true;

            var fullPath = Path.Combine(
                _env.WebRootPath,
                user.ProfileImagePath.TrimStart('/')
            );

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            user.ProfileImagePath = null;
            await _Irepository.UpdateAsync(user);

            return true;
        }


    }
}
