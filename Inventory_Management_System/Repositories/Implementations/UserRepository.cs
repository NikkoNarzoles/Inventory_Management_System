using Inventory_Management_System.Data;
using Inventory_Management_System.DTOs;
using Inventory_Management_System.Models;
using Inventory_Management_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Inventory_Management_System.Repositories.Implementations
{
    public class UserRepository : IUserRepository   
    {



        private readonly InventoryDbContext _context;

        public UserRepository(InventoryDbContext context)
        {
            _context = context;
        }




        public async Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u =>
                    u.username == usernameOrEmail ||
                    u.email == usernameOrEmail
                );
        }





        //=================================================================================================================
        //=================================================================================================================


        public async Task<bool> AnyAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users.AnyAsync(predicate);
        }


        //=================================================================================================================
        //=================================================================================================================


        public async Task<User?> FindAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }


        //=================================================================================================================
        //=================================================================================================================


        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            return await _context.Users
                .AsNoTracking() // important for read-only performance
                .Where(u => u.role != "Admin")
                .Select(
                item => new UserDto
                {   
                    id = item.id,
                    first_name = item.first_name,
                    last_name = item.last_name,
                    username = item.username,
                }
                ).ToListAsync();
        }


        //=================================================================================================================
        //=================================================================================================================


        // GET BY ID (async)
        public async Task<UserDto?> GetByIdAsync(int id)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(item => item.id == id)
                .Select(item => new UserDto
                {
                    id = item.id,
                    first_name = item.first_name,
                    last_name = item.last_name,
                    username = item.username,
                })
                .FirstOrDefaultAsync();
        }


        //=================================================================================================================
        //=================================================================================================================



        // ADD (async)
        public async Task AddAsync(User item)
        {
            await _context.Users.AddAsync(item);
            await _context.SaveChangesAsync();
        }



        //=================================================================================================================
        //=================================================================================================================




        // UPDATE (async)
        public async Task UpdateAsync(User item)
        {
            _context.Users.Update(item);
            await _context.SaveChangesAsync();
        }



        //=================================================================================================================
        //=================================================================================================================




        // DELETE (async)
        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.Users.FindAsync(id);

            if (item == null)
                return false;

            _context.Users.Remove(item);
            await _context.SaveChangesAsync();

            return true;
        }



    }
}
