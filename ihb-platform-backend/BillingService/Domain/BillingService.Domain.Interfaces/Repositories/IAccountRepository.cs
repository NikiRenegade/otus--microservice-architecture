using BillingService.Domain.Entities;

namespace BillingService.Domain.Interfaces.Repositories;

public interface IAccountRepository
{
    Task<IEnumerable<Account>> GetAllAsync();
    Task<Account?> GetByUserIdAsync(Guid userId);
    Task<Account?> GetByUserEmailAsync(string email);
    Task<Account?> AddAsync(Account account);
    Task<bool> UpdateAsync(Guid id, Account account);
    Task<bool> UpdateEmailAsync(Guid id, string email);
    
    Task<bool> DeleteAsync(Guid id);
}