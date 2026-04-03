using UserService.Domain.Events;

namespace UserService.Domain.Interfaces.Publishers;

/// <summary>
/// Интерфейс для публикации событий, связанных с пользователями.
/// Отвечает за отправку событий создания, обновления и удаления пользователей в message broker.
/// </summary>
public interface IUserEventPublisher
{
    /// <summary>
    /// Публикует событие создания нового пользователя.
    /// </summary>
    /// <param name="userCreatedEvent">Событие с данными о созданном пользователе.</param>
    /// <returns>Задача публикации события.</returns>
    public Task PublishUserCreated(UserCreatedEvent userCreatedEvent);

    /// <summary>
    /// Публикует событие обновления данных пользователя.
    /// </summary>
    /// <param name="userUpdatedEvent">Событие с данными об обновлённом пользователе.</param>
    /// <returns>Задача публикации события.</returns>
    public Task PublishUserUpdated(UserUpdatedEvent userUpdatedEvent);

    /// <summary>
    /// Публикует событие удаления пользователя.
    /// </summary>
    /// <param name="userId">ID удаляемого пользователя.</param>
    /// <returns>Задача публикации события.</returns>
    public Task PublishUserDeleted(Guid userId);
}