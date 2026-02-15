using UserService.Application.Mappers;
using UserService.Domain.DTOs;
using UserService.Domain.Interfaces.Repositories;
using UserService.Domain.Interfaces.Services;

namespace UserService.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<IList<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        if (users == null)
            return new List<UserDto>();
        return users.Select(u=> u.ToUserDto()).ToList();
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user?.ToUserDto();
    }

    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user?.ToUserDto();
    }

    public async Task<UserDto> AddAsync(UserRegisterDto dto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new InvalidOperationException($"Пользователь с email '{dto.Email}' уже существует.");

        var user = dto.ToEntity();

        var created = await _userRepository.AddAsync(user, dto.Password);
        if (created==null)
            throw new InvalidOperationException("Не удалось создать пользователя.");
        return created.ToUserDto();
    }

    public async Task<bool> UpdateAsync(Guid id, UserUpdateDto dto)
    {
        return await _userRepository.UpdateAsync(id, dto.ToEntity());
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _userRepository.DeleteAsync(id);
    }
}