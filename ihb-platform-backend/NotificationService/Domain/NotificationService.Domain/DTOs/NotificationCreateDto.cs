namespace NotificationService.Domain.DTOs;

public class NotificationCreateDto
{
    public Guid UserId { get; set; }
    public string Text { get; set; } = string.Empty;
}