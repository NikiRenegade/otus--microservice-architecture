using InventoryService.Domain.DTOs;

namespace InventoryService.Domain.Interfaces.Services;

public interface IInventoryService
{
    Task<bool> Reserve(ReserveOrderItemDto  reserveOrderItemDto);
    Task Release(Guid orderId);
    Task<ProductStockDto> AddProductStock(AddProductStockDto dto);
    Task<ProductStockDto?> GetProductStock(Guid productId);
    Task<List<ProductStockDto?>?> GetAllProductStocks();
}