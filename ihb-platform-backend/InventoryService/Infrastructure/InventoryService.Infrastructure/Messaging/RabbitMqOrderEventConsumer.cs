using InventoryService.Domain.DTOs;
using InventoryService.Domain.Interfaces.Services;
using Shared.RabbitMq.Interfaces;

namespace InventoryService.Infrastructure.Messaging;

public class RabbitMqOrderEventConsumer: IRabbitMqConsumer
{
    private readonly IEventConsumer _consumer;
    private readonly IInventoryService _inventoryService;
    private const string ExchangeName = "order-events";

    public RabbitMqOrderEventConsumer(IEventConsumer consumer, IInventoryService inventoryService)
    {
        _consumer = consumer;
        _inventoryService = inventoryService;
    }

    public async Task StartAsync()
    {
        await _consumer.SubscribeAsync<ReserveOrderItemDto>("order", "order.product.reserve", ExchangeName,
            async @event => { await _inventoryService.Reserve(@event); });
    }
}