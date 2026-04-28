using InventoryService.Domain.Entities;
using InventoryService.Domain.Interfaces.Repositories;
using InventoryService.Infrastructure.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Infrastructure.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly InventoryDbContext _context;
    public  InventoryRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductStock>?> GetAllProductStocks()
    {
        var productStocks = await _context.ProductStocks.ToListAsync();
        return productStocks;
    }

    public async Task<ProductStock?> GetProductStock(Guid productId)
    {
        return await _context.ProductStocks.FirstOrDefaultAsync(x => x.ProductId == productId);
    }

    public async Task DecreaseProductStock(Guid productId, int quantity)
    {
        var productStock = await _context.ProductStocks.FirstOrDefaultAsync(x => x.ProductId == productId);
        if (productStock == null)
            throw  new Exception("Product stock not found");
        productStock.AvailableQuantity -= quantity;
    }

    public async Task IncreaseProductStock(Guid productId, int quantity)
    {
        var productStock = await _context.ProductStocks.FirstOrDefaultAsync(x => x.ProductId == productId);
        if (productStock == null)
            throw  new Exception("Product stock not found");
        productStock.AvailableQuantity += quantity;
    }
    

    public Task AddProductStockReservation(ProductStockReservation reservation)
    {
        _context.ProductStockReservations.Add(reservation);
        return Task.CompletedTask;
    }

    public async Task<List<ProductStockReservation>> GetProductStockReservations(Guid orderId)
    {
        return await _context.ProductStockReservations.Where(x => x.OrderId == orderId)
            .ToListAsync();
    }

    public async Task<bool> RemoveReservations(Guid orderId)
    {
        var items = await _context.ProductStockReservations
            .Where(x => x.OrderId == orderId)
            .ToListAsync();
        if (!items.Any())
        {
            return false;
        }
        
        _context.ProductStockReservations.RemoveRange(items);
        return true;
    }

    public async Task<bool> HasProductStockReservation(Guid orderId)
    {
        return await _context.ProductStockReservations.AnyAsync(x => x.OrderId == orderId);
    }
    
    public async Task<ProductStock> CreateProductStock(ProductStock stock)
    {
        await _context.ProductStocks.AddAsync(stock);
        await _context.SaveChangesAsync();
        return stock;
    }
}