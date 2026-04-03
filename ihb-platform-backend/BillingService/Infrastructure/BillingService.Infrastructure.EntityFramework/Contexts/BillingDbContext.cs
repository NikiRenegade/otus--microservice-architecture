using BillingService.Domain.Entities;
using BillingService.Infrastructure.EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BillingService.Infrastructure.EntityFramework.Contexts;

/// <summary>
/// Контекст базы данных Entity Framework для Billing Service.
/// Управляет операциями базы данных для счетов и платежей.
/// </summary>
public class BillingDbContext : DbContext
{
    /// <summary>
    /// Получает или устанавливает коллекцию счетов.
    /// </summary>
    public DbSet<Account> Accounts => Set<Account>();
    
    /// <summary>
    /// Получает или устанавливает коллекцию платежей.
    /// </summary>
    public DbSet<Payment> Payments => Set<Payment>();

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="BillingDbContext"/>.
    /// </summary>
    /// <param name="options">Параметры контекста базы данных.</param>
    public BillingDbContext(DbContextOptions<BillingDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration<Account>(new AccountConfiguration());
        modelBuilder.ApplyConfiguration<Payment>(new PaymentConfiguration());
    }
}