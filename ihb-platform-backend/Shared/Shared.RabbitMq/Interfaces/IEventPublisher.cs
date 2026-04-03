using System;

namespace Shared.RabbitMq.Interfaces;

/// <summary>
/// Интерфейс для публикации событий в RabbitMQ брокер сообщений.
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Публикует событие в указанный exchange с заданным routing key.
    /// </summary>
    /// <typeparam name="T">Тип события (должен быть сериализуемым в JSON).</typeparam>
    /// <param name="event">Объект события для публикации.</param>
    /// <param name="routingKey">Routing key для маршрутизации события.</param>
    /// <param name="exchangeName">Имя exchange, в который публиковать событие.</param>
    /// <returns>Задача публикации (выполняется асинхронно).</returns>
    Task PublishAsync<T>(T @event, string routingKey, string exchangeName);
}
