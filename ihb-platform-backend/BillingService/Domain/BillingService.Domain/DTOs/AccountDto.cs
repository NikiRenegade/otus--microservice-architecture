namespace BillingService.Domain.DTOs;

public class AccountDto
{
    public Guid UserId { get; set; }
    public string? UserEmail { get; set; }
    public decimal? Balance { get; set; }
}