using InventoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryService.Infrastructure.EntityFramework.Configurations;

public class ProductStockReservationConfiguration : IEntityTypeConfiguration<ProductStockReservation>
{
    public void Configure(EntityTypeBuilder<ProductStockReservation> builder)
    {
        builder.ToTable("ProductStockReservations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OrderId)
            .IsRequired();
        builder.Property(x => x.OrderId)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .IsRequired();
        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}