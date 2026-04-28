namespace BillingService.Domain.Entities;

/// <summary>
/// Представляет счет для выставления счетов, связанный с пользователем.
/// </summary>
public class Account
{
    /// <summary>
    /// Уникальный идентификатор счета.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Уникальный идентификатор пользователя, владельца этого счета.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Email адрес, связанный с этим счетом.
    /// </summary>
    public string UserEmail { get; set; } = string.Empty;
    
    /// <summary>
    /// Текущий баланс счета.
    /// </summary>
    public decimal? Balance { get; set; }
}