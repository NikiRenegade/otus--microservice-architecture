using UserService.Domain.DTOs;
using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces.Services;

public interface IUserService
{
    Task<IList<UserDto>> GetAllAsync();

    Task<UserDto?> GetByIdAsync(Guid id);
    
    Task<UserDto?> GetByEmailAsync(string email);

    Task<UserDto> AddAsync(UserRegisterDto dto);

    Task<bool> UpdateAsync(Guid id,UserUpdateDto dto);

    Task<bool> DeleteAsync(Guid id);
}