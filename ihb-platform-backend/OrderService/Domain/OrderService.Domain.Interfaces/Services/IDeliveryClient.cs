namespace OrderService.Domain.Interfaces.Services;

/// <summary>
/// Клиент для коммуникации с Delivery Service.
/// </summary>
public interface IDeliveryClient
{
    Task<bool> Reserve(Guid orderId, Guid userId, DateTime timeSlot);
    Task Cancel(Guid orderId,  Guid userId);
}