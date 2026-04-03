using BillingService.Domain.Entities;
using BillingService.Infrastructure.EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BillingService.Infrastructure.EntityFramework.Contexts;

public class BillingDbContext : DbContext
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Payment> Payments => Set<Payment>();

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