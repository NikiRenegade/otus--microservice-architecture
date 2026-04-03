namespace Shared.RabbitMq.Interfaces;

/// <summary>
/// Интерфейс для подписки на события из RabbitMQ брокера сообщений.
/// </summary>
public interface IEventConsumer
{
    /// <summary>
    /// Подписывает обработчик на события определённого типа с заданным routing key.
    /// </summary>
    /// <typeparam name="T">Тип события (должен быть сериализуемым).</typeparam>
    /// <param name="name">Имя потребителя.</param>
    /// <param name="routingKey">Routing key для фильтрации событий.</param>
    /// <param name="exchangeName">Имя exchange, из которого читать события.</param>
    /// <param name="handleEvent">Асинхронный обработчик события.</param>
    /// <returns>Задача подписки (выполняется асинхронно).</returns>
    Task SubscribeAsync<T>(string name, string routingKey, string exchangeName, Func<T, Task> handleEvent);
}