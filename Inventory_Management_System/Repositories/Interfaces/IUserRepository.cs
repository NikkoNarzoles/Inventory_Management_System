using Inventory_Management_System.DTOs;
using Inventory_Management_System.Models;
using System.Linq.Expressions;

namespace Inventory_Management_System.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<User?> FindAsync(int id);
        Task AddAsync(User item);
        Task UpdateAsync(User item);
        Task <bool> DeleteAsync(int id);
        Task<bool> AnyAsync(Expression<Func<User, bool>> predicate);

        Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail);

        Task<List<UserDto>> SearchAsync(string search);

        Task<bool> DeleteprofileAsync(int userId);


    }
}       
