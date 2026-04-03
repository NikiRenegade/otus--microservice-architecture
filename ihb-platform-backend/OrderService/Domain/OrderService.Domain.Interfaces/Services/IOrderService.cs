using OrderService.Domain.DTOs;

namespace OrderService.Domain.Interfaces.Services;

/// <summary>
/// Интерфейс для управления операциями заказов.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Создает новый заказ на основе предоставленных данных заказа.
    /// </summary>
    /// <param name="orderCreateDto">Объект трансфера данных для создания заказа.</param>
    /// <returns>Объект трансфера данных ответа, содержащий подробности созданного заказа.</returns>
    Task<OrderCreateResponseDto> CreateOrder(OrderCreateDto orderCreateDto);
}