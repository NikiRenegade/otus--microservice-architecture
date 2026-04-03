namespace OrderService.Domain.DTOs;

/// <summary>
/// DTO для создания нового заказа.
/// </summary>
public class OrderCreateDto
{
    /// <summary>
    /// Уникальный идентификатор пользователя, разместившего заказ.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Коллекция товаров для включения в заказ.
    /// </summary>
    public List<OrderItemDto> Items { get; set; } = new();
}