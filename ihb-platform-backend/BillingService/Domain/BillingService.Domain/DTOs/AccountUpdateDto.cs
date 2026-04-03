namespace BillingService.Domain.DTOs;

/// <summary>
/// DTO для обновления информации счета.
/// </summary>
public class AccountUpdateDto
{
    /// <summary>
    /// Новый адрес электронной почты для счета.
    /// </summary>
    public string? UserEmail { get; set; }
    
    /// <summary>
    /// Новый баланс счета.
    /// </summary>
    public decimal Balance { get; set; }
}