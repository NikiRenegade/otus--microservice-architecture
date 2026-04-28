namespace Shared.ServiceToken.Interfaces;

/// <summary>
/// Интерфейс для генерации JWT токенов для межсервисного взаимодействия.
/// </summary>
public interface IServiceTokenGenerator
{
    /// <summary>
    /// Генерирует JWT токен для сервиса с указанными параметрами.
    /// </summary>
    /// <param name="serviceName">Имя сервиса, от имени которого генерируется токен.</param>
    /// <param name="userId">ID пользователя, к которому относится токен.</param>
    /// <param name="audience">Целевая аудитория токена (сервис-получатель).</param>
    /// <param name="scope">Область действия токена (разрешения).</param>
    /// <returns>Подписанный JWT токен.</returns>
    string Generate(string serviceName, Guid userId, string audience, string scope);
}