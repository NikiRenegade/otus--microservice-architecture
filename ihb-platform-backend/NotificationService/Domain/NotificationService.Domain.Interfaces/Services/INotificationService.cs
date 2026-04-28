using NotificationService.Domain.DTOs;

namespace NotificationService.Domain.Interfaces.Services;

public interface INotificationService
{
    /// <summary>
    /// Возвращает список всех уведомлений в виде DTO.
    /// </summary>
    /// <returns>Список уведомлений.</returns>
    Task<IList<NotificationDto>> GetAllAsync();

    /// <summary>
    /// Создаёт уведомление на основе DTO.
    /// </summary>
    /// <param name="dto">Данные для создания уведомления.</param>
    /// <returns>Созданное уведомление в виде DTO.</returns>
    Task<NotificationDto> AddAsync(NotificationCreateDto dto);
}