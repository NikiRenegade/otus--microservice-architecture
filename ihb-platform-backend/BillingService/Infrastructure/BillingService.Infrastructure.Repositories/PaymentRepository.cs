using BillingService.Domain.Entities;
using BillingService.Domain.Interfaces.Repositories;
using BillingService.Infrastructure.EntityFramework.Contexts;

namespace BillingService.Infrastructure.Repositories;

/// <summary>
/// Репозиторий для управления операциями сохранения записей о платежах.
/// </summary>
public class PaymentRepository : IPaymentRepository
{
    private readonly BillingDbContext _context;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="PaymentRepository"/>.
    /// </summary>
    /// <param name="context">Контекст базы данных выставления счетов.</param>
    public PaymentRepository(BillingDbContext context)
    {
        _context = context;
    }
    
    public async Task<Payment?> AddAsync(Payment payment)
    {
        _context.Payments.Add(payment);

        var result = await _context.SaveChangesAsync();
        
        if (result == 0)
        {
            return null;
        }
        return payment;
    }
}