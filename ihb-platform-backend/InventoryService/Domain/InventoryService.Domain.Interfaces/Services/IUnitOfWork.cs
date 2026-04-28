namespace InventoryService.Domain.Interfaces.Services;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}