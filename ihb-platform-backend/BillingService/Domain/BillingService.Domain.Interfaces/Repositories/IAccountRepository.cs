using BillingService.Domain.Entities;

namespace BillingService.Domain.Interfaces.Repositories;

public interface IAccountRepository
{
    Task<IEnumerable<Account>> GetAllAsync();
    Task<Account?> GetByUserIdAsync(Guid userId);
    Task<Account?> GetByUserEmailAsync(string email);
    Task<Account?> AddAsync(Account account);
    Task<bool> UpdateAsync(Guid userId, Account account);
    Task<bool> UpdateEmailAsync(Guid userId, string email);
    
    Task<bool> DeleteAsync(Guid userId);
}