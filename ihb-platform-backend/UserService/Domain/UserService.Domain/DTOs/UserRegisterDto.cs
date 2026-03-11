namespace UserService.Domain.DTOs;

/// <summary>
/// DTO для регистрации нового пользователя.
/// </summary>
/// <param name="Email">Email пользователя.</param>
/// <param name="Password">Пароль в открытом виде.</param>
/// <param name="UserName">Логин пользователя.</param>
/// <param name="FirstName">Имя пользователя.</param>
/// <param name="LastName">Фамилия пользователя.</param>
public record UserRegisterDto(
    string Email,
    string Password,
    string UserName,
    string FirstName,
    string LastName
);