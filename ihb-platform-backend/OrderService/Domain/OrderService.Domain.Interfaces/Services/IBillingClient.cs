namespace OrderService.Domain.Interfaces.Services;

public interface IBillingClient
{
    Task<(bool Success, Guid? PaymentId)> Withdraw(Guid userId, decimal amount);
}