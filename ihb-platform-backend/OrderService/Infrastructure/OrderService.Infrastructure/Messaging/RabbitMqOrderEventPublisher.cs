using OrderService.Domain.Events;
using OrderService.Domain.Interfaces.Publishers;
using Shared.RabbitMq.Interfaces;
using OrderService.Domain.Events;
using OrderService.Domain.Interfaces.Publishers;

namespace OrderService.Infrastructure.Messaging;

public class RabbitMqOrderEventPublisher : IOrderEventPublisher
{
    private readonly IEventPublisher _eventPublisher;
    private const string ExchangeName = "order-events";
    

    public RabbitMqOrderEventPublisher(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }
    
    public Task PublishOrderCompleted(OrderСompletedEvent userCreatedEvent)
    {
        return _eventPublisher.PublishAsync(
            userCreatedEvent,
            routingKey: "order.completed",
            exchangeName: ExchangeName
        );
    }
}