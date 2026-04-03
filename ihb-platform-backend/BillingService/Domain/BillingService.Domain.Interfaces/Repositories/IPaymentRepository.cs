using BillingService.Domain.Entities;

namespace BillingService.Domain.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task<Payment?> AddAsync(Payment payment);
}