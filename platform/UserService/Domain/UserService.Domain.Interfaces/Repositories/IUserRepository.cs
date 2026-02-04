using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);

    Task<User?> AddAsync(User user, string password);

    Task<bool> UpdateAsync(User user);

    Task<bool> DeleteAsync(Guid id);
}