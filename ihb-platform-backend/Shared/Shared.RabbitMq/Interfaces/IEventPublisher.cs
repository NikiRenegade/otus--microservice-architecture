using System;

namespace Shared.RabbitMq.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<T>(T @event, string routingKey, string exchangeName);
}
