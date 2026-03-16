using BillingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillingService.Infrastructure.EntityFramework.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(x => x.UserId);
        builder.Property(x => x.UserEmail)
            .HasMaxLength(256)
            .IsRequired();
        builder.Property(x => x.Balance)
            .IsRequired();
    }
}