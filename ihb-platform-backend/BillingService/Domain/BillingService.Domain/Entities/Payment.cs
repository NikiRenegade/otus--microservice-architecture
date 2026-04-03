namespace BillingService.Domain.Entities;

public class Payment
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public decimal Amount { get; set; }

    public PaymentStatus Status { get; set; }
    
    public PaymentType Type { get; set; }

    public DateTime CreatedAt { get; set; }
}

public enum PaymentType
{
    Deposit,
    Withdraw
}
public enum PaymentStatus
{
    Success,
    Failed
}