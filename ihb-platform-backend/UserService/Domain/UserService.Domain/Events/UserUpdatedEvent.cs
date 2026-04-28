namespace UserService.Domain.Events;

public class UserUpdatedEvent
{
    public Guid Id { get; set; } 
    public string Email { get; set; }
}