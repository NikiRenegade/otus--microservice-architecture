using BillingService.Domain.Entities;
using BillingService.Domain.Interfaces.Repositories;
using BillingService.Infrastructure.EntityFramework.Contexts;

namespace BillingService.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly BillingDbContext _context;

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