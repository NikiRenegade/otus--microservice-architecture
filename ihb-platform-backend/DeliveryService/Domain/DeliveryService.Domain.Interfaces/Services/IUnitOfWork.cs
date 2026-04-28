namespace DeliveryService.Domain.Interfaces.Services;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}