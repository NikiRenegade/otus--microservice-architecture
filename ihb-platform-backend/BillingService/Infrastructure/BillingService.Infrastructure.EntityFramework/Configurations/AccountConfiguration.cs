using BillingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillingService.Infrastructure.EntityFramework.Configurations;

/// <summary>
/// Конфигурация Entity Framework для сущности Account.
/// Определяет структуру таблицы, ключи, ограничения и конфигурации свойств.
/// </summary>
public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    /// <summary>
    /// Настраивает отображение сущности Account.
    /// </summary>
    /// <param name="builder">Построитель типа сущности для настройки свойств Account.</param>
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.HasIndex(x => x.UserId)
            .IsUnique();

        builder.Property(x => x.UserEmail)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.Balance)
            .HasColumnType("decimal(18,2)");
    }
}