namespace UserService.Domain.Events;

/// <summary>
/// Событие, публикуемое при создании нового пользователя.
/// Используется для межсервисной коммуникации через RabbitMQ.
/// </summary>
public class UserCreatedEvent
{
    /// <summary>
    /// Уникальный идентификатор созданного пользователя.
    /// </summary>
    public Guid UserId { get; set; } 
    
    /// <summary>
    /// Email адрес созданного пользователя.
    /// </summary>
    public string Email { get; set; }
}