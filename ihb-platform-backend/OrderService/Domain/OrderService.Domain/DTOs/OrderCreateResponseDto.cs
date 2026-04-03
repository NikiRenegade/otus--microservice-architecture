namespace OrderService.Domain.DTOs;

public class OrderCreateResponseDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public decimal Price { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    
    public Guid? PaymentId { get; set; }
}