using BillingService.Domain.Entities;

namespace BillingService.Domain.Interfaces.Repositories;

/// <summary>
/// Интерфейс репозитория для управления сущностями счетов.
/// </summary>
public interface IAccountRepository
{
    /// <summary>
    /// Получает все счета из базы данных.
    /// </summary>
    /// <returns>Перечисление всех счетов.</returns>
    Task<IEnumerable<Account>> GetAllAsync();
    
    /// <summary>
    /// Получает счет по идентификатору пользователя.
    /// </summary>
    /// <param name="userId">Уникальный идентификатор пользователя.</param>
    /// <returns>Счет, если найден; в противном случае <c>null</c>.</returns>
    Task<Account?> GetByUserIdAsync(Guid userId);
    
    /// <summary>
    /// Получает счет по адресу электронной почты пользователя.
    /// </summary>
    /// <param name="email">Email для поиска.</param>
    /// <returns>Счет, если найден; в противном случае <c>null</c>.</returns>
    Task<Account?> GetByUserEmailAsync(string email);
    
    /// <summary>
    /// Добавляет новый счет в базу данных.
    /// </summary>
    /// <param name="account">Сущность счета для добавления.</param>
    /// <returns>Добавленный счет, если успешно; в противном случае <c>null</c>.</returns>
    Task<Account?> AddAsync(Account account);
    
    /// <summary>
    /// Обновляет существующий счет в базе данных.
    /// </summary>
    /// <param name="userId">Уникальный идентификатор пользователя, чей счет обновляется.</param>
    /// <param name="account">Сущность счета с обновленной информацией.</param>
    /// <returns><c>true</c>, если обновление было успешным; в противном случае <c>false</c>.</returns>
    Task<bool> UpdateAsync(Guid userId, Account account);
    
    /// <summary>
    /// Обновляет адрес электронной почты счета.
    /// </summary>
    /// <param name="userId">Уникальный идентификатор пользователя, чей email обновляется.</param>
    /// <param name="email">Новый адрес электронной почты.</param>
    /// <returns><c>true</c>, если обновление было успешным; в противном случае <c>false</c>.</returns>
    Task<bool> UpdateEmailAsync(Guid userId, string email);
    
    /// <summary>
    /// Удаляет счет из базы данных.
    /// </summary>
    /// <param name="userId">Уникальный идентификатор пользователя, чей счет удаляется.</param>
    /// <returns><c>true</c>, если удаление было успешным; в противном случае <c>false</c>.</returns>
    Task<bool> DeleteAsync(Guid userId);
}