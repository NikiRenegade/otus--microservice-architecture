namespace OrderService.Domain.Events;

public class OrderСompletedEvent
{
    public Guid UserId { get; set; } 
    public string Text { get; set; } = string.Empty;
}