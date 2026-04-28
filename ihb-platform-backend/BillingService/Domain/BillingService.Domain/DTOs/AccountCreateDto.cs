namespace BillingService.Domain.DTOs;

/// <summary>
/// DTO для создания нового счета.
/// </summary>
public class AccountCreateDto
{
    /// <summary>
    /// Уникальный идентификатор пользователя для нового счета.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Адрес электронной почты для нового счета.
    /// </summary>
    public string Email { get; set; }
}