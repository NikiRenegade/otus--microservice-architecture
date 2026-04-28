namespace UserService.Domain.DTOs;

/// <summary>
/// Data Transfer Object для аутентификации пользователя.
/// Содержит учётные данные для входа в систему.
/// </summary>
public class UserLoginDto
{
    /// <summary>
    /// Email адрес пользователя для входа.
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    public string Password { get; set; }
}