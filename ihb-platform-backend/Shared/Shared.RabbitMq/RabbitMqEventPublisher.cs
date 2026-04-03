using System;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Shared.RabbitMq.Interfaces;

namespace Shared.RabbitMq;

/// <summary>
/// Реализация издателя событий для RabbitMQ брокера сообщений.
/// Публикует события в topic exchange.
/// </summary>
public class RabbitMqEventPublisher : IEventPublisher
{
    /// <summary>
    /// Задача асинхронного подключения к каналу RabbitMQ.
    /// </summary>
    private readonly Task<IChannel> _channelTask;

    /// <summary>
    /// Кеш объявленных exchange для избежания повторных объявлений.
    /// </summary>
    private readonly ConcurrentDictionary<string, bool> _exchanges = new();

    /// <summary>
    /// Кеш объявленных очередей (для будущего расширения функционала).
    /// </summary>
    private readonly ConcurrentDictionary<string, bool> _queues = new();

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="RabbitMqEventPublisher"/>.
    /// </summary>
    /// <param name="channelTask">Задача, возвращающая готовый канал RabbitMQ.</param>
    public RabbitMqEventPublisher(Task<IChannel> channelTask)
    {
        _channelTask = channelTask;
    }

    /// <summary>
    /// Асинхронно публикует событие в RabbitMQ с указанным routing key.
    /// Автоматически объявляет exchange при первой публикации.
    /// </summary>
    /// <typeparam name="T">Тип события.</typeparam>
    /// <param name="event">Объект события.</param>
    /// <param name="routingKey">Routing key для маршрутизации (например, "order.created").</param>
    /// <param name="exchangeName">Имя topic exchange для публикации.</param>
    /// <exception cref="InvalidOperationException">Выбрасывается если канал RabbitMQ недоступен.</exception>
    public async Task PublishAsync<T>(T @event, string routingKey, string exchangeName)
    {
        var channel = await _channelTask;

        if (channel is null)
            throw new InvalidOperationException("Канал RabbitMQ null");

        try
        {
            if (_exchanges.TryAdd(exchangeName, true))
            {
                await channel.ExchangeDeclareAsync(
                    exchange: exchangeName,
                    type: ExchangeType.Topic,
                    durable: true
                );
            }

            var json = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(
                exchange: exchangeName,
                routingKey: routingKey,
                body: body
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка в PublishAsync: {ex}");
            throw;
        }
    }
}
