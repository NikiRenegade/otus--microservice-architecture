using NotificationService.Domain.DTOs;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Interfaces.Repositories;
using NotificationService.Domain.Interfaces.Services;

namespace NotificationService.Application.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }


    public async Task<IList<NotificationDto>> GetAllAsync()
    {
        var notifications = await _notificationRepository.GetAllAsync();
        if (notifications == null)
            return new List<NotificationDto>();
        return notifications.Select(n =>
        {
            return new NotificationDto
                { Id = n.Id, UserId = n.UserId, Text = n.Text };
        }).ToList();
    }

    public async Task<NotificationDto> AddAsync(NotificationCreateDto dto)
    {
        var notification = new Notification { UserId = dto.UserId, Text = dto.Text };

        var created = await _notificationRepository.AddAsync(notification);
        if (created == null)
            throw new InvalidOperationException("Не удалось создать уведомление.");
        return new NotificationDto { Id = created.Id, UserId = created.UserId, Text = created.Text };
    }
}