using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces.Services;

/// <summary>
/// Сервис для идентификации пользователя
/// </summary>
public interface IIdentityService
{
    Task<bool> SignInAsync(User user, string password);
    Task<User?> RegisterAsync(User user, string password);
}