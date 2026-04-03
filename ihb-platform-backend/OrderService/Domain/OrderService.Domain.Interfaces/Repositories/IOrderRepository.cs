using OrderService.Domain.Entities;

namespace OrderService.Domain.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<Order?> AddAsync(Order order);
    Task<Order?> UpdatePaymentAndStatusAsync(Guid id, Guid? paymentId, OrderStatus status);
}