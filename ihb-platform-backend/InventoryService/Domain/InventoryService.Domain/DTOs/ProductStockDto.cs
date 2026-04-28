namespace InventoryService.Domain.DTOs;

public class ProductStockDto
{
    public Guid ProductId { get; set; }
    public int AvailableQuantity { get; set; }
}