namespace OrderService.Domain.Interfaces.Services;

/// <summary>
/// Клиент для коммуникации с Billing Service.
/// </summary>
public interface IBillingClient
{
    /// <summary>
    /// Просит снятие на счет пользователя в Billing Service.
    /// </summary>
    /// <param name="userId">Уникальный идентификатор пользователя.</param>
    /// <param name="amount">Сумма для снятия.</param>
    /// <returns>Кортеж, индицирующий статус успеха и идентификатор платежа если статус успеха.</returns>
    Task<(bool Success, Guid? PaymentId)> Withdraw(Guid userId, decimal amount);
}