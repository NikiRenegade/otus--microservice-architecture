namespace BillingService.Domain.DTOs;

public class AccountChangeEmailDto
{
    public Guid UserId { get; set; }
    public string UserEmail { get; set; } = string.Empty;
}