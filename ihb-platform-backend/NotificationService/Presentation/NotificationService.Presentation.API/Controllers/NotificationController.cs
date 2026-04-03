using Microsoft.AspNetCore.Mvc;
using NotificationService.Domain.DTOs;
using NotificationService.Domain.Interfaces.Services;

namespace NotificationService.Presentation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    
    /// <summary>
    /// Возвращает все уведомления.
    /// </summary>
    /// <returns>Список уведомлений в виде DTO.</returns>
    [HttpGet]
    public async Task<ActionResult<IList<NotificationDto>>> GetAll()
    {
        try
        {
            var notifications = await _notificationService.GetAllAsync();
            return Ok(notifications);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка сервера при получении списка уведомлений", error = ex.Message });
        }
    }
}