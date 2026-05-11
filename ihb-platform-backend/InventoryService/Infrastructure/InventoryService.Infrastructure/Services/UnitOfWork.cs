using InventoryService.Domain.Interfaces.Services;
using InventoryService.Infrastructure.EntityFramework.Contexts;

namespace InventoryService.Infrastructure.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly InventoryDbContext _context;

    public UnitOfWork(InventoryDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}