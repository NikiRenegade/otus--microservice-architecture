using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.EntityFramework.Configurations;

/// <summary>
/// Конфигурация для Entity Framework Core для сущности User.
/// Определяет схему таблицы, ограничения, индексы и свойства.
/// </summary>
public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// Конфигурирует сущность User для отображения на таблицу базы данных.
    /// Устанавливает имя таблицы, первичный ключ, ограничения на длину полей,
    /// уникальные индексы для Email, UserName и PhoneNumber.
    /// </summary>
    /// <param name="builder">Builder для конфигурации сущности User.</param>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(u => u.PhoneNumber)
            .IsRequired(false)
            .HasMaxLength(25);

        builder.HasIndex(u => u.UserName).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.PhoneNumber).IsUnique();
    }
}