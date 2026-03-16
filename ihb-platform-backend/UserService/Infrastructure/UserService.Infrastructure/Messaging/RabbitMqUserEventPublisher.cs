using Shared.RabbitMq.Interfaces;
using UserService.Domain.Events;
using UserService.Domain.Interfaces.Publishers;

namespace UserService.Infrastructure.Messaging;

public class RabbitMqUserEventPublisher : IUserEventPublisher
{
    private readonly IEventPublisher _eventPublisher;
    private const string ExchangeName = "user-events";
    

    public RabbitMqUserEventPublisher(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }
    
    public Task PublishUserCreated(UserCreatedEvent userCreatedEvent)
    {
        return _eventPublisher.PublishAsync(
            userCreatedEvent,
            routingKey: "user.created",
            exchangeName: ExchangeName
        );
    }

    public Task PublishUserUpdated(UserUpdatedEvent userUpdatedEvent)
    {
        return _eventPublisher.PublishAsync(
            @userUpdatedEvent,
            routingKey: "user.emailchange",
            exchangeName: ExchangeName
        );
    }

    public Task PublishUserDeleted(Guid userId)
    {
        return _eventPublisher.PublishAsync(
            userId,
            routingKey: "user.deleted",
            exchangeName: ExchangeName
        );
    }
}