namespace OrderService.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public decimal Price { get; set; }

    public OrderStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public Guid? PaymentId { get; set; }
    
    public List<OrderItem> Items { get; set; } = new();
}

public enum OrderStatus
{
    Created,
    Pending,
    Failed,
    Completed,
}