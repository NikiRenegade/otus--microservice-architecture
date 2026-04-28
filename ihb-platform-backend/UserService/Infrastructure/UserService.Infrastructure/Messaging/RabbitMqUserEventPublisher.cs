using Shared.RabbitMq.Interfaces;
using UserService.Domain.Events;
using UserService.Domain.Interfaces.Publishers;

namespace UserService.Infrastructure.Messaging;

/// <summary>
/// Реализация издателя событий пользователя через RabbitMQ.
/// Публикует события создания, обновления и удаления пользователей
/// в message broker.
/// </summary>
public class RabbitMqUserEventPublisher : IUserEventPublisher
{
    /// <summary>
    /// Издатель событий RabbitMQ для отправки сообщений в message broker.
    /// </summary>
    private readonly IEventPublisher _eventPublisher;

    /// <summary>
    /// Имя exchange для всех событий пользователя.
    /// </summary>
    private const string ExchangeName = "user-events";

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="RabbitMqUserEventPublisher"/>.
    /// </summary>
    /// <param name="eventPublisher">Издатель событий RabbitMQ.</param>
    public RabbitMqUserEventPublisher(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    /// <summary>
    /// Асинхронно публикует событие создания пользователя.
    /// Routing key: "user.created"
    /// </summary>
    /// <param name="userCreatedEvent">Событие с данными о созданном пользователе.</param>
    /// <returns>Задача публикации события.</returns>
    public Task PublishUserCreated(UserCreatedEvent userCreatedEvent)
    {
        return _eventPublisher.PublishAsync(
            userCreatedEvent,
            routingKey: "user.created",
            exchangeName: ExchangeName
        );
    }

    /// <summary>
    /// Асинхронно публикует событие обновления данных пользователя.
    /// Routing key: "user.emailchange"
    /// </summary>
    /// <param name="userUpdatedEvent">Событие с данными об обновлённом пользователе.</param>
    /// <returns>Задача публикации события.</returns>
    public Task PublishUserUpdated(UserUpdatedEvent userUpdatedEvent)
    {
        return _eventPublisher.PublishAsync(
            @userUpdatedEvent,
            routingKey: "user.emailchange",
            exchangeName: ExchangeName
        );
    }

    /// <summary>
    /// Асинхронно публикует событие удаления пользователя.
    /// Routing key: "user.deleted"
    /// </summary>
    /// <param name="userId">ID удаляемого пользователя.</param>
    /// <returns>Задача публикации события.</returns>
    public Task PublishUserDeleted(Guid userId)
    {
        return _eventPublisher.PublishAsync(
            userId,
            routingKey: "user.deleted",
            exchangeName: ExchangeName
        );
    }
}