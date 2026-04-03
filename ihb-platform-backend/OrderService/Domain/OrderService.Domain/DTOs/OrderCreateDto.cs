namespace OrderService.Domain.DTOs;

public class OrderCreateDto
{
    public Guid UserId { get; set; }

    public List<OrderItemDto> Items { get; set; } = new();
}