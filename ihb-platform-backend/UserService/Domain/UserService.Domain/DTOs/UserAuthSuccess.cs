namespace UserService.Domain.DTOs;

/// <summary>
/// DTO, возвращаемый при успешной аутентификации пользователя.
/// Содержит JWT токен доступа и информацию о пользователе.
/// </summary>
public record UserAuthSuccess
{
    /// <summary>
    /// JWT токен доступа для дальнейших запросов к API.
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Информация о аутентифицированном пользователе.
    /// </summary>
    public UserDto UserDto { get; set; }
}