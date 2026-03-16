using BillingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillingService.Infrastructure.EntityFramework.Contexts;

public class BillingDbContext : DbContext
{
    public DbSet<Account> Accounts => Set<Account>();

    public BillingDbContext(DbContextOptions<BillingDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BillingDbContext).Assembly);
    }
}