namespace BillingService.Domain.DTOs;

/// <summary>
/// DTO для информации о счете.
/// </summary>
public class AccountDto
{
    /// <summary>
    /// Уникальный идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Email адрес пользователя.
    /// </summary>
    public string? UserEmail { get; set; }
    
    /// <summary>
    /// Текущий баланс счета.
    /// </summary>
    public decimal? Balance { get; set; }
}