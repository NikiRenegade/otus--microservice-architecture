namespace BillingService.Domain.Entities;

public class Account
{
    public Guid UserId { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public decimal? Balance { get; set; }
}