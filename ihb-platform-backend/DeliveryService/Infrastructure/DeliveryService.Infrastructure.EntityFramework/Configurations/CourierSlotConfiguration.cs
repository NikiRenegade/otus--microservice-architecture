using DeliveryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryService.Infrastructure.EntityFramework.Configurations;

public class CourierSlotConfiguration : IEntityTypeConfiguration<CourierSlot>
{
    public void Configure(EntityTypeBuilder<CourierSlot> builder)
    {
        builder.ToTable("CourierSlots");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.TimeSlot).IsRequired();
        
        builder.Property(x=> x.CourierId).IsRequired();
        
        builder.HasOne(x => x.Courier)
            .WithMany(x => x.Slots)
            .HasForeignKey(x => x.CourierId);
    }
}