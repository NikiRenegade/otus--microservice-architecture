using DeliveryService.Domain.Entities;
using DeliveryService.Infrastructure.EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Infrastructure.EntityFramework.Contexts;

public class DeliveryDbContext: DbContext
{

    public DbSet<Courier> Couriers => Set<Courier>();
    
    public DbSet<CourierSlot> CourierSlots => Set<CourierSlot>();
    
    public DbSet<DeliveryReservation> DeliveryReservations => Set<DeliveryReservation>();

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DeliveryDbContext"/>.
    /// </summary>
    /// <param name="options">Параметры контекста базы данных.</param>
    public DeliveryDbContext(DbContextOptions<DeliveryDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration<Courier>(new CourierConfiguration());
        modelBuilder.ApplyConfiguration<CourierSlot>(new CourierSlotConfiguration());
        modelBuilder.ApplyConfiguration<DeliveryReservation>(new DeliveryReservationConfiguration());
    }
    
}