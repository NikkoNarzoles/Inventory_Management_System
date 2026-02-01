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
        private readonly IProfileRepository _Iprofile;

        public UserRepository(InventoryDbContext context, IProfileRepository iprofile)
        {
            _context = context;
            _Iprofile = iprofile;
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
                .Select((u => new UserDto
                {
                    id = u.id,
                    first_name = u.first_name,
                    last_name = u.last_name,
                    username = u.username,

                    ItemCount = _context.StoreItems.Count(i => i.owners_id == u.id)
                })
                ).ToListAsync();
        }


        public async Task<List<UserDto>> SearchAsync(string search)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u =>
                    u.first_name.Contains(search) ||
                    u.last_name.Contains(search) ||
                    u.username.Contains(search)
                )
                .Select(u => new UserDto
                {
                    id = u.id,
                    first_name = u.first_name,
                    last_name = u.last_name,
                    username = u.username,
                    ItemCount = _context.StoreItems.Count(i => i.owners_id == u.id)
                })
                .ToListAsync();
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
                    theme_id = item.theme_id
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

            var ownItems = await _context.StoreItems
                     .Where(s => s.owners_id == id)
                     .ToListAsync();

            _context.Users.Remove(item);
            _context.StoreItems.RemoveRange(ownItems);
            await _context.SaveChangesAsync();

            return true;
        }


        //=================================================================================================================
        //=================================================================================================================




    }
}
