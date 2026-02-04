using UserService.Domain.DTOs;
using UserService.Domain.Entities;

namespace UserService.Application.Mappers;

public static class UserMappers
{
    public static User ToEntity(this UserRegisterDto dto) => new User
    {
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        UserName = dto.UserName,
        Email = dto.Email,
    };
    public static User ToEntity(this UserUpdateDto dto) => new User
    {
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        UserName = dto.UserName,
        Email = dto.Email,
    };

    public static UserDto ToUserDto(this User user) => new UserDto
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        UserName = user.UserName,
        Email = user.Email,
    };
}