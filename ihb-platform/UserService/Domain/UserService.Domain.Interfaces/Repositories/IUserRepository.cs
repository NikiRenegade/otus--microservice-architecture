using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces.Repositories;

/// <summary>
/// Репозиторий для работы с <see cref="User"/>.
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Поиск пользователя по Email.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    /// <returns>Пользователь или <c>null</c>, если не найден.</returns>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Обновляет поля существующего пользователя.
    /// </summary>
    /// <param name="id">Идентификатор обновляемого пользователя.</param>
    /// <param name="user">Новые значения полей пользователя.</param>
    /// <returns><c>true</c>, если обновление выполнено; иначе <c>false</c>.</returns>
    Task<bool> UpdateAsync(Guid id, User user);

    /// <summary>
    /// Удаляет пользователя по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns><c>true</c>, если удаление выполнено; иначе <c>false</c>.</returns>
    Task<bool> DeleteAsync(Guid id);
}