namespace OrderService.Domain.Entities;

/// <summary>
/// Представляет отдельный товар в заказе.
/// </summary>
public class OrderItem
{
    /// <summary>
    /// Уникальный идентификатор единицы заказа.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор заказа, который содержит этот товар.
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Идентификатор товара.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Количество товара в заказt.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Цена за единицу товара.
    /// </summary>
    public decimal Price { get; set; }
}