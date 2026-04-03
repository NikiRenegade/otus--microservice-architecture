using NotificationService.Domain.DTOs;
using NotificationService.Domain.Interfaces.Services;
using Shared.RabbitMq.Interfaces;

namespace NotificationService.Infrastructure.Messaging;

public class RabbitMqOrderEventConsumer : IRabbitMqConsumer
{
    private readonly IEventConsumer _consumer;
    private readonly INotificationService _notificationService;
    private const string ExchangeName = "order-events";

    public RabbitMqOrderEventConsumer(IEventConsumer consumer, INotificationService notificationService)
    {
        _consumer = consumer;
        _notificationService = notificationService;
    }

    public async Task StartAsync()
    {
        await _consumer.SubscribeAsync<NotificationCreateDto>("order", "order.completed", ExchangeName,
            async @event => { await _notificationService.AddAsync(@event); });
    }
}