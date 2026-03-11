namespace UserService.Domain.Interfaces.Repositories;

/// <summary>
/// Общий интерфейс репозитория для базовых операций над сущностями.
/// </summary>
/// <typeparam name="T">Тип сущности.</typeparam>
public interface IRepository<T>
{
    /// <summary>
    /// Возвращает все сущности заданного типа.
    /// </summary>
    /// <returns>Перечисление сущностей или <c>null</c>, если список пуст.</returns>
    Task<IEnumerable<T>?> GetAllAsync();

    /// <summary>
    /// Возвращает сущность по её идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>Сущность или <c>null</c>, если не найдена.</returns>
    Task<T?> GetByIdAsync(Guid id);

}