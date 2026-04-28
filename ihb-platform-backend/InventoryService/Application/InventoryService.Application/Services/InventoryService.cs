using InventoryService.Domain.DTOs;
using InventoryService.Domain.Entities;
using InventoryService.Domain.Interfaces.Repositories;
using InventoryService.Domain.Interfaces.Services;

namespace InventoryService.Application.Services;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public InventoryService(IInventoryRepository inventoryRepository,  IUnitOfWork unitOfWork)
    {
        _inventoryRepository = inventoryRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> Reserve(ReserveOrderItemDto dto)
    {
        if (await _inventoryRepository.HasProductStockReservation(dto.OrderId))
            return true;

        foreach (var item in dto.Items)
        {
            var stock = await _inventoryRepository.GetProductStock(item.ProductId);

            if (stock == null || stock.AvailableQuantity < item.Quantity)
                return false;
        }
        
        foreach (var item in dto.Items)
        {
            await _inventoryRepository.DecreaseProductStock(item.ProductId, item.Quantity);

            await _inventoryRepository.AddProductStockReservation(new ProductStockReservation
            {
                Id = Guid.NewGuid(),
                OrderId = dto.OrderId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                CreatedAt = DateTime.UtcNow
            });
        }

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task Release(Guid orderId)
    {
        var reservations = await _inventoryRepository.GetProductStockReservations(orderId);

        foreach (var r in reservations)
        {
            await _inventoryRepository.IncreaseProductStock(r.ProductId, r.Quantity);
        }

        await _inventoryRepository.RemoveReservations(orderId);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task<ProductStockDto> AddProductStock(AddProductStockDto dto)
    {
        var productStock = await _inventoryRepository.GetProductStock(dto.ProductId);

        if (productStock == null)
        {
            productStock = await _inventoryRepository.CreateProductStock(new ProductStock
            {
                ProductId = dto.ProductId,
                AvailableQuantity = dto.Quantity
            });
        }
        else
        {
            productStock.AvailableQuantity += dto.Quantity;
        }
        
        return new ProductStockDto { 
            ProductId = productStock.ProductId,
            AvailableQuantity = productStock.AvailableQuantity }; 
    }

    public async Task<ProductStockDto?> GetProductStock(Guid productId)
    {
        var productStock = await _inventoryRepository.GetProductStock(productId);
        return new ProductStockDto { 
            ProductId = productId,
            AvailableQuantity = productStock.AvailableQuantity };
    }
    public async Task<List<ProductStockDto>?> GetAllProductStocks()
    {
        var productStock = await _inventoryRepository.GetAllProductStocks();
        return productStock.Select(x => 
            new ProductStockDto
            {
                ProductId = x.ProductId,
                AvailableQuantity = x.AvailableQuantity
            }
        ).ToList();
    }
}