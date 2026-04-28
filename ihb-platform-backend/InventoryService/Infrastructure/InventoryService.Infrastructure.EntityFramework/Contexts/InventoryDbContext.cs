using InventoryService.Domain.Entities;
using InventoryService.Infrastructure.EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Infrastructure.EntityFramework.Contexts;

public class InventoryDbContext : DbContext
{

    public DbSet<ProductStock> ProductStocks => Set<ProductStock>();
    
    public DbSet<ProductStockReservation> ProductStockReservations => Set<ProductStockReservation>();

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="InventoryDbContext"/>.
    /// </summary>
    /// <param name="options">Параметры контекста базы данных.</param>
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration<ProductStock>(new ProductStockConfiguration());
        modelBuilder.ApplyConfiguration<ProductStockReservation>(new ProductStockReservationConfiguration());
    }
    
}