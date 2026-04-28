using InventoryService.Domain.Entities;

namespace InventoryService.Domain.Interfaces.Repositories;

public interface IInventoryRepository
{
    Task<List<ProductStock>?> GetAllProductStocks();
    Task<ProductStock?> GetProductStock(Guid productId);
    Task DecreaseProductStock(Guid productId, int quantity);
    Task IncreaseProductStock(Guid productId, int quantity);

    Task AddProductStockReservation(ProductStockReservation reservation);
    Task<List<ProductStockReservation>> GetProductStockReservations(Guid orderId);
    Task<bool> RemoveReservations(Guid orderId);

    Task<bool> HasProductStockReservation(Guid orderId);
    Task<ProductStock> CreateProductStock(ProductStock stock);
}