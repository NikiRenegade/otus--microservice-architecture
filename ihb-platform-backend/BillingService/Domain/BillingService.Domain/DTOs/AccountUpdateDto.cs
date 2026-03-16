namespace BillingService.Domain.DTOs;

public class AccountUpdateDto
{
    public string? UserEmail { get; set; }
    public decimal Balance { get; set; }
}