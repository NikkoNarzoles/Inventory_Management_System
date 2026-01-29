using Inventory_Management_System.DTOs;
using Inventory_Management_System.Models;
using Inventory_Management_System.Repositories.Interfaces;
using Inventory_Management_System.Services.ServiceInterface;
using Inventory_Management_System.ViewModels;



namespace Inventory_Management_System.Services.ServicesImplementation
{
    public class UserService : IUserService
    {


        private readonly IUserRepository _Irepository;

        public UserService (IUserRepository UserRepository)
        {
            _Irepository = UserRepository;
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
                theme_id = 0,

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
            user.email = vm.email;

            await _Irepository.UpdateAsync(user);
            return true;
        }






    }
}
