using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces.Repositories;
using UserService.Infrastructure.EntityFramework.Contexts;

namespace UserService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(UserDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<User>?> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> UpdateAsync(User entity)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == entity.Id);
            if (existingUser == null)
                return false;

            existingUser.FirstName = entity.FirstName;
            existingUser.LastName = entity.LastName;
            existingUser.Email = entity.Email;
            existingUser.UserName = entity.UserName;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
            if (existingUser == null)
                return false;
            _context.Users.Remove(existingUser);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        
        public async Task<User?> AddAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
                return _userManager.Users.FirstOrDefault(u => u.Email == user.Email);
            return null;
        }
    }
}