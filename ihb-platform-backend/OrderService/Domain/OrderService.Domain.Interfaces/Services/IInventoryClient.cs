using OrderService.Domain.DTOs;

namespace OrderService.Domain.Interfaces.Services;

/// <summary>
/// Клиент для коммуникации с Inventory Service.
/// </summary>
public interface IInventoryClient
{
    Task<bool> Reserve(Guid orderId, Guid userId, List<OrderItemDto> items);
    Task Release(Guid orderId, Guid userId);
}