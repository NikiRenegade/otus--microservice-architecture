using BillingService.Domain.DTOs;
using BillingService.Domain.Entities;
using BillingService.Domain.Interfaces.Repositories;
using BillingService.Domain.Interfaces.Services;

namespace BillingService.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IPaymentRepository _paymentRepository;

    public AccountService(IAccountRepository accountRepository, IPaymentRepository paymentRepository)
    {
        _accountRepository = accountRepository;
        _paymentRepository = paymentRepository;
    }
    
    
    public async Task<IList<AccountDto>> GetAllAsync()
    {
        var accounts = await _accountRepository.GetAllAsync();
        if (accounts == null)
            return new List<AccountDto>();
        return accounts.Select(a =>
        { 
            return new AccountDto 
                { UserId = a.UserId, UserEmail = a.UserEmail, Balance = a.Balance };
        }).ToList();
    }

    public async Task<AccountDto?> GetByIdAsync(Guid id)
    {
        var account = await _accountRepository.GetByUserIdAsync(id);
        return account == null ? new AccountDto() : new AccountDto 
            { 
                UserId = account.UserId, 
                UserEmail = account.UserEmail,
                Balance = account.Balance 
            };
    }

    public async Task<AccountDto?> GetByEmailAsync(string email)
    {
        var account = await _accountRepository.GetByUserEmailAsync(email);
        return account == null ? new AccountDto() : new AccountDto 
        { 
            UserId = account.UserId, 
            UserEmail = account.UserEmail,
            Balance = account.Balance 
        };
    }

    public async Task<AccountDto> AddAsync(AccountCreateDto dto)
    {
        var existingUser = await _accountRepository.GetByUserEmailAsync(dto.Email);
        if (existingUser != null)
            throw new InvalidOperationException($"Пользователь с email '{dto.Email}' уже существует.");

        var account = new Account { UserId = dto.UserId, UserEmail = dto.Email, Balance = 0};

        var created = await _accountRepository.AddAsync(account);
        if (created == null)
            throw new InvalidOperationException("Не удалось создать аккаунт.");
        return new AccountDto() 
            {UserId = created.UserId, UserEmail = created.UserEmail, Balance = created.Balance};
    }

    public async Task<bool> UpdateAsync(Guid id, AccountUpdateDto dto)
    {
        return await _accountRepository.UpdateAsync(id, new Account 
            {UserEmail = dto.UserEmail, Balance = dto.Balance});
    }

    public async Task<bool> UpdateEmailAsync(Guid id, string email)
    {
        return await _accountRepository.UpdateAsync(id, new Account 
            {UserEmail = email});
    }

    public async Task<(bool Success, Guid PaymentId)> DepositAsync(Guid userId, decimal amount)
    {
         var account = await _accountRepository.GetByUserIdAsync(userId);
        
            if (account == null)
                return (false, Guid.Empty);
        
            account.Balance += amount;

            var result =  await _accountRepository.UpdateAsync(userId, account);

            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Amount = amount,
                Type = PaymentType.Deposit,
                Status = result ? PaymentStatus.Success : PaymentStatus.Failed,
                CreatedAt = DateTime.UtcNow
            };

            await _paymentRepository.AddAsync(payment);

            return (result, payment.Id);
    }

    public async Task<(bool Success, Guid PaymentId)> WithdrawAsync(Guid userId, decimal amount)
    {
        var account = await _accountRepository.GetByUserIdAsync(userId);
        
        if (account == null)
            return (false, Guid.Empty);
        
        if (account.Balance < amount)
        {
            var failedPayment = new Payment
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Amount = amount,
                Type = PaymentType.Withdraw,
                Status = PaymentStatus.Failed,
                CreatedAt = DateTime.UtcNow
            };

            await _paymentRepository.AddAsync(failedPayment);

            return (false, failedPayment.Id);
        }

        account.Balance -= amount;

        await _accountRepository.UpdateAsync(userId, account);

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Amount = amount,
            Type = PaymentType.Withdraw,
            Status = PaymentStatus.Failed,
            CreatedAt = DateTime.UtcNow
        };

        await _paymentRepository.AddAsync(payment);

        return (true, payment.Id);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _accountRepository.DeleteAsync(id);
    }
}