using OrderService.Domain.Entities;

namespace OrderService.Domain.Interfaces.Repositories;

/// <summary>
/// Интерфейс репозитория для управления сущностями заказов.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Добавляет новый заказ в базу данных.
    /// </summary>
    /// <param name="order">Сущность заказа для добавления.</param>
    /// <returns>Добавленный заказ, если успешно; в противном случае <c>null</c>.</returns>
    Task<Order?> AddAsync(Order order);
    
    /// <summary>
    /// Обновляет азсоциацию платежа и статус заказа.
    /// </summary>
    /// <param name="id">Уникальный идентификатор заказа.</param>
    /// <param name="paymentId">Идентификатор платежа для установки.</param>
    /// <param name="status">Новый статус заказа.</param>
    /// <returns>Обновленный заказ, если успешно; в противном случае <c>null</c>.</returns>
    Task<Order?> UpdatePaymentAndStatusAsync(Guid id, Guid? paymentId, OrderStatus status);
}