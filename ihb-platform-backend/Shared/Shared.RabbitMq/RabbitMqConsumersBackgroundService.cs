using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.RabbitMq.Interfaces;

namespace Shared.RabbitMq;

/// <summary>
/// Фоновый сервис для запуска всех потребителей RabbitMQ событий.
/// Автоматически запускается при старте приложения.
/// </summary>
public class RabbitMqConsumersBackgroundService : BackgroundService
{
    /// <summary>
    /// Провайдер сервисов для разрешения всех зарегистрированных потребителей RabbitMQ.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="RabbitMqConsumersBackgroundService"/>.
    /// </summary>
    /// <param name="serviceProvider">Провайдер сервисов приложения.</param>
    public RabbitMqConsumersBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Выполняет инициализацию и запуск всех зарегистрированных RabbitMQ потребителей.
    /// Сервис остается активным до остановки приложения.
    /// </summary>
    /// <param name="stoppingToken">Токен отмены для остановки сервиса.</param>
    /// <returns>Задача, выполняющаяся в фоне.</returns>
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