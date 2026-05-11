using DeliveryService.Domain.Interfaces.Services;
using DeliveryService.Infrastructure.EntityFramework.Contexts;

namespace DeliveryService.Infrastructure.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly DeliveryDbContext _context;

    public UnitOfWork(DeliveryDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}