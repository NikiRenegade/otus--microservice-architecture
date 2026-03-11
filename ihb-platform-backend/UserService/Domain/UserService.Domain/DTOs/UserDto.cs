namespace UserService.Domain.DTOs;

/// <summary>
/// DTO для представления публичных данных пользователя.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Email пользователя.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Логин пользователя.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия пользователя.
    /// </summary>
    public string LastName { get; set; }
}