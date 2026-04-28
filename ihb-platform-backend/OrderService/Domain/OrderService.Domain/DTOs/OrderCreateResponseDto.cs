namespace OrderService.Domain.DTOs;

/// <summary>
/// DTO для ответа после создания заказа.
/// </summary>
public class OrderCreateResponseDto
{
    /// <summary>
    /// Уникальный идентификатор созданного заказа.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Уникальный идентификатор пользователя, который разместил заказ.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Общая цена заказа.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Статус заказа в виде текстового представления.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Дата и время создания заказа.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Идентификатор связанного платежа, если он есть.
    /// </summary>
    public Guid? PaymentId { get; set; }
}