using UserService.Domain.DTOs;
using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces.Services;

/// <summary>
/// Интерфейс сервиса для работы с пользователями.
/// Оперирует DTO-объектами.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Возвращает список всех пользователей в виде DTO.
    /// </summary>
    /// <returns>Список пользователей.</returns>
    Task<IList<UserDto>> GetAllAsync();

    /// <summary>
    /// Возвращает пользователя по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>Пользователь в виде DTO или <c>null</c>, если не найден.</returns>
    Task<UserDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Возвращает пользователя по email.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    /// <returns>Пользователь в виде DTO или <c>null</c>, если не найден.</returns>
    Task<UserDto?> GetByEmailAsync(string email);

    /// <summary>
    /// Создаёт пользователя на основе регистрационной DTO.
    /// </summary>
    /// <param name="dto">Данные для регистрации пользователя.</param>
    /// <returns>Созданный пользователь в виде DTO.</returns>
    Task<UserDto> AddAsync(UserRegisterDto dto);

    /// <summary>
    /// Обновляет данные пользователя.
    /// </summary>
    /// <param name="id">Идентификатор обновляемого пользователя.</param>
    /// <param name="dto">DTO с новыми данными.</param>
    /// <returns><c>true</c>, если обновление выполнено; иначе <c>false</c>.</returns>
    Task<bool> UpdateAsync(Guid id, UserUpdateDto dto);

    /// <summary>
    /// Удаляет пользователя по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя для удаления.</param>
    /// <returns><c>true</c>, если удаление выполнено; иначе <c>false</c>.</returns>
    Task<bool> DeleteAsync(Guid id);
}