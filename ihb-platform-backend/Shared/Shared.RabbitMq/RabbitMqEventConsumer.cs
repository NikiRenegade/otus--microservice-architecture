using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.RabbitMq.Interfaces;

namespace Shared.RabbitMq;

/// <summary>
/// Реализация подписчика событий для RabbitMQ брокера сообщений.
/// Обеспечивает автоматическое объявление exchange и очередей, привязку и обработку сообщений.
/// </summary>
public class RabbitMqEventConsumer : IEventConsumer
{
    /// <summary>
    /// Задача асинхронного подключения к каналу RabbitMQ.
    /// </summary>
    private readonly Task<IChannel> _channelTask;

    /// <summary>
    /// Кеш объявленных очередей для избежания повторных объявлений.
    /// </summary>
    private readonly ConcurrentDictionary<string, bool> _queues = new();

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="RabbitMqEventConsumer"/>.
    /// </summary>
    /// <param name="channelTask">Задача, возвращающая готовый канал RabbitMQ.</param>
    public RabbitMqEventConsumer(Task<IChannel> channelTask)
    {
        _channelTask = channelTask;
    }

    /// <summary>
    /// Асинхронно подписывает обработчик на события из RabbitMQ.
    /// Автоматически создает exchange, очередь и привязывает их к routing key.
    /// Событие десериализуется из JSON и передается в обработчик.
    /// </summary>
    /// <typeparam name="T">Тип события для десериализации.</typeparam>
    /// <param name="name">Имя потребителя.</param>
    /// <param name="routingKey">Routing key для фильтрации событий.</param>
    /// <param name="exchangeName">Имя topic exchange.</param>
    /// <param name="handleEvent">Асинхронный обработчик события.</param>
    /// <returns>Задача подписки.</returns>
    public async Task SubscribeAsync<T>(string name, string routingKey, string exchangeName, Func<T, Task> handleEvent)
    {
        var channel = await _channelTask;

        if (_queues.TryAdd(routingKey, true))
        {
            await channel.ExchangeDeclareAsync(
                exchange: exchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false);

            string queueName = $"{name}_{routingKey.Replace(".", "_")}.queue";

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            await channel.QueueBindAsync(
                queue: queueName,
                exchange: exchangeName,
                routingKey: routingKey);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (sender, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var eventObj = JsonSerializer.Deserialize<T>(message);

                    if (eventObj != null)
                        await handleEvent(eventObj);

                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Во время обработки сообщения произошла ошибка: {ex}");
                }
            };

            await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
        }
    }
}