namespace BillingService.Domain.DTOs;

/// <summary>
/// DTO для изменения адреса электронной почты счета.
/// </summary>
public class AccountChangeEmailDto
{
    /// <summary>
    /// Уникальный идентификатор пользователя, чья почта изменяется.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Новый адрес электронной почты для счета.
    /// </summary>
    public string UserEmail { get; set; } = string.Empty;
}