using OrderService.Domain.Events;
namespace OrderService.Domain.Interfaces.Publishers;

public interface IOrderEventPublisher
{
    public Task PublishOrderCompleted(OrderСompletedEvent userCreatedEvent);
}