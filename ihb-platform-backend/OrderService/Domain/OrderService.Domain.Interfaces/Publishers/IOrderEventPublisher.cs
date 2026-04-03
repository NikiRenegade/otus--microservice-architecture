using OrderService.Domain.Events;
namespace OrderService.Domain.Interfaces.Publishers;

public interface IOrderEventPublisher
{
    /// <summary>
    /// Публикует событие завершения заказа.
    /// </summary>
    /// <param name="userCreatedEvent">Событие о завершению заказа для публикации.</param>
    /// <returns>Объект, репрезентирующий асинхронную операцию.</returns>
    Task PublishOrderCompleted(OrderСompletedEvent userCreatedEvent);
}