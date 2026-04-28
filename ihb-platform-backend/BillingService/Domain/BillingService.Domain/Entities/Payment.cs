namespace BillingService.Domain.Entities;

/// <summary>
/// Представляет транзакцию платежа в системе выставления счетов.
/// </summary>
public class Payment
{
    /// <summary>
    /// Уникальный идентификатор платежа.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Уникальный идентификатор пользователя, который произвел платеж.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Сумма платежа.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Текущий статус платежа.
    /// </summary>
    public PaymentStatus Status { get; set; }
    
    /// <summary>
    /// Тип платежа (Пополнение или Снятие).
    /// </summary>
    public PaymentType Type { get; set; }

    /// <summary>
    /// Дата и время создания платежа.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Перечисление, представляющее тип платежной транзакции.
/// </summary>
public enum PaymentType
{
    /// <summary>Деньги пополнены на счет.</summary>
    Deposit,
    /// <summary>Деньги сняты со счета.</summary>
    Withdraw
}

/// <summary>
/// Перечисление, представляющее статус платежной транзакции.
/// </summary>
public enum PaymentStatus
{
    /// <summary>Платеж выполнен успешно.</summary>
    Success,
    /// <summary>Платеж не выполнен.</summary>
    Failed
}