namespace InventoryService.Domain.DTOs;

public class AddProductStockDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}