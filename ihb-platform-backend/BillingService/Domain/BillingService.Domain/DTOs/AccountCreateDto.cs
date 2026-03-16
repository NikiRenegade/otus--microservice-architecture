namespace BillingService.Domain.DTOs;

public class AccountCreateDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
}