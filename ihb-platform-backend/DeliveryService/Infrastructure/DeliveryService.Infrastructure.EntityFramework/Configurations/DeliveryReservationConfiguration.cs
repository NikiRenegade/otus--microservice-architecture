using DeliveryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryService.Infrastructure.EntityFramework.Configurations;

public class DeliveryReservationConfiguration : IEntityTypeConfiguration<DeliveryReservation>
{
    public void Configure(EntityTypeBuilder<DeliveryReservation> builder)
    {
        builder.ToTable("DeliveryReservations");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.OrderId).IsRequired();
        
        builder.Property(x=> x.CourierSlotId).IsRequired();
        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("NOW()");
        
        builder.HasOne(x => x.CourierSlot)
            .WithMany(x => x.Reservations)
            .HasForeignKey(x => x.CourierSlotId);
    }
}