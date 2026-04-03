namespace OrderService.Domain.Entities;

/// <summary>
/// Представляет заказ, разместивший пользователь.
/// </summary>
public class Order
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Уникальный идентификатор пользователя, заказавшего.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Общая цена заказа.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Текущий статус заказа.
    /// </summary>
    public OrderStatus Status { get; set; }

    /// <summary>
    /// Дата и время создания заказа.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Идентификатор связанного платежа, если он есть.
    /// </summary>
    public Guid? PaymentId { get; set; }
    
    /// <summary>
    /// Коллекция товаров в заказе.
    /// </summary>
    public List<OrderItem> Items { get; set; } = new();
}

/// <summary>
/// Перечисление, представляющее статус заказа.
/// </summary>
public enum OrderStatus
{
    /// <summary>Заказ только что создан.</summary>
    Created,
    /// <summary>Обработка платежа или заказа не успешна.</summary>
    Failed,
    /// <summary>Заказ исполнен успешно.</summary>
    Completed,
}