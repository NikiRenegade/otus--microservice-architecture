namespace Shared.RabbitMq.Interfaces;

public interface IRabbitMqConsumer
{
    Task StartAsync();
}