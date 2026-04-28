namespace Shared.RabbitMq.Interfaces;

/// <summary>
/// Интерфейс для запуска потребителей событий RabbitMQ.
/// </summary>
public interface IRabbitMqConsumer
{
    /// <summary>
    /// Запускает потребителя для обработки входящих сообщений из RabbitMQ.
    /// </summary>
    /// <returns>Задача, которая выполняется до остановки приложения.</returns>
    Task StartAsync();
}