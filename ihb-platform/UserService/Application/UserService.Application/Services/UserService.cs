using UserService.Application.Mappers;
using UserService.Domain.DTOs;
using UserService.Domain.Interfaces.Repositories;
using UserService.Domain.Interfaces.Services;

namespace UserService.Application.Services;

/// <summary>
/// Сервис логики для работы с пользователями.
/// Делегирует работу репозиторию.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Конструктор сервиса.
    /// </summary>
    /// <param name="userRepository">Репозиторий пользователей.</param>
    /// <param name="jwtTokenService">Сервис генерации jwt.</param>
    /// <param name="IIdentityService">Сервис идентификации.</param>
    public UserService(IUserRepository userRepository, IJwtTokenService jwtTokenService, IIdentityService identityService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _identityService = identityService;
    }

    /// <summary>
    /// Возвращает всех пользователей в виде DTO.
    /// </summary>
    public async Task<IList<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        if (users == null)
            return new List<UserDto>();
        return users.Select(u => u.ToUserDto()).ToList();
    }

    /// <summary>
    /// Возвращает пользователя по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>DTO пользователя или <c>null</c>.</returns>
    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user?.ToUserDto();
    }

    /// <summary>
    /// Возвращает пользователя по email.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user?.ToUserDto();
    }

    /// <summary>
    /// Создаёт пользователя и возвращает DTO созданного пользователя.
    /// </summary>
    /// <param name="dto">Данные для регистрации пользователя.</param>
    /// <returns>Созданный пользователь в виде DTO.</returns>
    public async Task<UserDto> RegisterAsync(UserRegisterDto dto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new InvalidOperationException($"Пользователь с email '{dto.Email}' уже существует.");

        var user = dto.ToEntity();

        var created = await _identityService.RegisterAsync(user, dto.Password);
        if (created == null)
            throw new InvalidOperationException("Не удалось создать пользователя.");
        return created.ToUserDto();
    }

    /// <summary>
    /// Обновляет данные пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="dto">DTO с новыми данными.</param>
    /// <returns><c>true</c>, если обновление выполнено; иначе <c>false</c>.</returns>
    public async Task<bool> UpdateAsync(Guid id, UserUpdateDto dto)
    {
        return await _userRepository.UpdateAsync(id, dto.ToEntity());
    }

    /// <summary>
    /// Удаляет пользователя по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns><c>true</c>, если удаление выполнено; иначе <c>false</c>.</returns>
    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _userRepository.DeleteAsync(id);
    }

    /// <summary>
    /// Проверяет попытку входа пользователя в систему
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>Пользователь в виде DTO с токеном или <c>null</c>, если попытка входа неудачна.</returns>
    public async Task<UserAuthSuccess?> LoginAsync(UserLoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        var result = await _identityService.SignInAsync(user, dto.Password);
        if (result == false)
        {
            return null;
        }
        var accessToken = _jwtTokenService.GenerateJwtToken(user);
        return new UserAuthSuccess
        {
            UserDto = user.ToUserDto(),
            AccessToken = accessToken,
        };
    }
}