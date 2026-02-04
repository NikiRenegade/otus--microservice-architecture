namespace UserService.Domain.Interfaces.Repositories;

public interface IRepository<T>
{
    Task<IEnumerable<T>?> GetAllAsync();

    Task<T?> GetByIdAsync(Guid id);
    
}