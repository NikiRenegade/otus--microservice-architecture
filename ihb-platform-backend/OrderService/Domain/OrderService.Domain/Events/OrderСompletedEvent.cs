namespace OrderService.Domain.Events;

/// <summary>
/// Событие, публикуемое когда заказ завершен.
/// </summary>
public class OrderСompletedEvent
{
    /// <summary>
    /// Уникальный идентификатор пользователя, который наложил заказ.
    /// </summary>
    public Guid UserId { get; set; } 
    
    /// <summary>
    /// Сообщение об завершении или описание.
    /// </summary>
    public string Text { get; set; } = string.Empty;
}