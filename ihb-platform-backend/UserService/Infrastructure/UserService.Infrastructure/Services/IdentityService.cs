using Microsoft.AspNetCore.Identity;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces.Services;

namespace UserService.Infrastructure.Services;

/// <summary>
/// Конкретная реализация IIdentityService, использующая <see cref="SignInManager{TUser}" />.
/// </summary>
public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;

    /// <summary>
    /// Конструктор IdentityService.
    /// </summary>
    /// <param name="signInManager">Менеджер работы с Identity-пользователями для попытки входа в систему.</param>
    /// <param name="userManager">Менеджер работы с Identity-пользователями.</param>
    public IdentityService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    /// <summary>
    /// Проверяет попытку входа в систему пользователя через <see cref="UserManager{TUser}" />.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<bool> SignInAsync(User user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }
    
    /// <summary>
    /// Создаёт пользователя через <see cref="UserManager{TUser}"/>.
    /// </summary>
    /// <param name="user">Сущность пользователя.</param>
    /// <param name="password">Пароль в открытом виде.</param>
    /// <returns>Созданный пользователь или <c>null</c>, если создание не удалось.</returns>
    public async Task<User?> RegisterAsync(User user, string password, string role)
    {
        var createResult = await _userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
            return null;

        var roleResult = await _userManager.AddToRoleAsync(user, role);

        if (!roleResult.Succeeded)
            return null;
        return _userManager.Users.FirstOrDefault(u => u.Email == user.Email);
    }

    public async Task<string?> GetUserRoleAsync(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Count == 0)
        {
            return string.Empty;
        }
        return roles.FirstOrDefault();
    }
}