using BillingService.Domain.Entities;
using BillingService.Domain.Interfaces.Repositories;
using BillingService.Infrastructure.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BillingService.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly BillingDbContext _context;

    public AccountRepository(BillingDbContext context)
    {
        _context = context;
    }

    public async Task<Account?> AddAsync(Account account)
    {
        _context.Accounts.Add(account);

        var result = await _context.SaveChangesAsync();
        
        if (result == 0)
        {
            return null;
        }
        return account;
    }

    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        return await _context.Accounts.ToListAsync();
    }

    public async Task<Account?> GetByUserIdAsync(Guid userId)
    {
        return await _context.Accounts
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }
    public async Task<Account?> GetByUserEmailAsync(string email)
    {
        return await _context.Accounts
            .FirstOrDefaultAsync(x => x.UserEmail == email);
    }

    public async Task<bool> UpdateAsync(Guid id, Account account)
    {
        var existingAccount = await _context.Accounts
            .FirstOrDefaultAsync(u => u.UserId == id);
        if (existingAccount == null)
            return false;
        existingAccount.UserEmail = account.UserEmail;
        existingAccount.Balance = account.Balance;
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> UpdateEmailAsync(Guid id, string email)
    {
        var existingAccount = await _context.Accounts
            .FirstOrDefaultAsync(u => u.UserId == id);
        if (existingAccount == null)
            return false;
        existingAccount.UserEmail = email;
        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<bool> DeleteAsync(Guid id)
    {
        var existingAccount = await _context.Accounts
            .FirstOrDefaultAsync(u => u.UserId == id);
        if (existingAccount == null)
            return false;
        _context.Accounts.Remove(existingAccount);
        await _context.SaveChangesAsync();
        return true;
    }
}