namespace InventoryService.Domain.DTOs;

public class ReserveOrderItemDto
{
    public Guid OrderId { get; set; }
    public List<OrderItemDto> Items { get; set; }
}