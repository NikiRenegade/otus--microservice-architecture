namespace UserService.Domain.DTOs;

/// <summary>
/// DTO для обновления данных пользователя.
/// </summary>
/// <param name="Email">Email пользователя.</param>
/// <param name="UserName">Логин/имя пользователя.</param>
/// <param name="FirstName">Имя пользователя.</param>
/// <param name="LastName">Фамилия пользователя.</param>
public record UserUpdateDto
(
     string Email,
     string UserName,
     string FirstName,
     string LastName
);