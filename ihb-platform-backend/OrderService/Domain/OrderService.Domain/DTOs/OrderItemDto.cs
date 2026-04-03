namespace OrderService.Domain.DTOs;

/// <summary>
/// DTO для товара в заказе.
/// </summary>
public class OrderItemDto
{
    /// <summary>
    /// Уникальный идентификатор товара.
    /// </summary>
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// Количество товара.
    /// </summary>
    public int Quantity { get; set; }
    
    /// <summary>
    /// Цена за единицу товара.
    /// </summary>
    public decimal Price { get; set; }
}