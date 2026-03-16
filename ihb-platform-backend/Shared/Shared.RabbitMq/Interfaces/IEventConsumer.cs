namespace Shared.RabbitMq.Interfaces;

public interface IEventConsumer
{
    Task SubscribeAsync<T>(string name, string routingKey, string exchangeName, Func<T, Task> handleEvent);
}