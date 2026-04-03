using BillingService.Domain.DTOs;

namespace BillingService.Domain.Interfaces.Services;

public interface IAccountService
{
    /// <summary>
    /// Возвращает список всех аккаунтов в виде DTO.
    /// </summary>
    /// <returns>Список аккаунтов.</returns>
    Task<IList<AccountDto>> GetAllAsync();

    /// <summary>
    /// Возвращает аккаунт по идентификатору пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>Пользователь в виде DTO или <c>null</c>, если не найден.</returns>
    Task<AccountDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Возвращает пользователя по email пользователя.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    /// <returns>Пользователь в виде DTO или <c>null</c>, если не найден.</returns>
    Task<AccountDto?> GetByEmailAsync(string email);

    /// <summary>
    /// Создаёт аккаунт на основе DTO.
    /// </summary>
    /// <param name="dto">Данные для создания аккаунта.</param>
    /// <returns>Созданный аккаунт в виде DTO.</returns>
    Task<AccountDto> AddAsync(AccountCreateDto dto);

    /// <summary>
    /// Обновляет данные аккаунта.
    /// </summary>
    /// <param name="id">Идентификатор пользователя обновляемого аккаунта.</param>
    /// <param name="dto">DTO с новыми данными.</param>
    /// <returns><c>true</c>, если обновление выполнено; иначе <c>false</c>.</returns>
    Task<bool> UpdateAsync(Guid id, AccountUpdateDto dto);
    
    /// <summary>
    /// Обновляет email аккаунта.
    /// </summary>
    /// <param name="id">Идентификатор пользователя обновляемого аккаунта.</param>
    /// <param name="dto">email аккаунта.</param>
    /// <returns><c>true</c>, если обновление выполнено; иначе <c>false</c>.</returns>
    Task<bool> UpdateEmailAsync(Guid id, string email);
    
    /// <summary>
    /// Пополнение аккаунта.
    /// </summary>
    /// <param name="id">Идентификатор пользователя пополняемого аккаунта.</param>
    /// <param name="amount">Сумма пополнения</param>
    /// <returns><c>true</c>, если пополнение выполнено; иначе <c>false</c>.</returns>
    Task<(bool Success, Guid PaymentId)> DepositAsync(Guid userId, decimal amount);
    
    /// <summary>
    /// Снятие средств со счета.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя счета, со которого снимаются средства.</param>
    /// <param name="amount">Сумма снятия.</param>
    /// <returns>Кортеж с результатом выполнения и идентификатором платежа.</returns>
    Task<(bool Success, Guid PaymentId)> WithdrawAsync(Guid userId, decimal amount);

    /// <summary>
    /// Удаляет аккаунт по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя для удаления аккаунта.</param>
    /// <returns><c>true</c>, если удаление выполнено; иначе <c>false</c>.</returns>
    Task<bool> DeleteAsync(Guid id);
}
