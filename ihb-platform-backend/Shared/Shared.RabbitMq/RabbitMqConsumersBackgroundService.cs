using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.RabbitMq.Interfaces;

namespace Shared.RabbitMq;

public class RabbitMqConsumersBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public RabbitMqConsumersBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var consumers = scope.ServiceProvider.GetServices<IRabbitMqConsumer>();

        foreach (var consumer in consumers)
        {
            _ = consumer.StartAsync();
        }

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}