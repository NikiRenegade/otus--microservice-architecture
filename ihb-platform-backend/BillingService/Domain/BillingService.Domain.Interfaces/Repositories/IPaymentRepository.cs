using BillingService.Domain.Entities;

namespace BillingService.Domain.Interfaces.Repositories;

/// <summary>
/// Интерфейс репозитория для управления сущностями платежей.
/// </summary>
public interface IPaymentRepository
{
    /// <summary>
    /// Добавляет новую запись о платеже в базу данных.
    /// </summary>
    /// <param name="payment">Сущность платежа для добавления.</param>
    /// <returns>Добавленный платеж, если успешно; в противном случае <c>null</c>.</returns>
    Task<Payment?> AddAsync(Payment payment);
}