using UserService.Domain.DTOs;
using UserService.Domain.Entities;

namespace UserService.Application.Mappers;
/// <summary>
/// Набор методов-расширений для преобразования между доменной сущностью <see cref="User"/>
/// и DTO-объектами.
/// </summary>
public static class UserMappers
{
    /// <summary>
    /// Преобразует <see cref="UserRegisterDto"/> в сущность <see cref="User"/> для создания нового пользователя.
    /// </summary>
    /// <param name="dto">DTO регистрации.</param>
    /// <returns>Новая сущность <see cref="User"/> с данными из DTO.</returns>
    public static User ToEntity(this UserRegisterDto dto) => new User
    {
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        UserName = dto.UserName,
        Email = dto.Email,
    };

    /// <summary>
    /// Преобразует <see cref="UserUpdateDto"/> в сущность <see cref="User"/> для обновления полей пользователя.
    /// </summary>
    /// <param name="dto">DTO обновления.</param>
    /// <returns>Сущность <see cref="User"/> с новыми значениями полей.</returns>
    public static User ToEntity(this UserUpdateDto dto) => new User
    {
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        UserName = dto.UserName,
        Email = dto.Email,
    };

    /// <summary>
    /// Преобразует доменную сущность <see cref="User"/> в <see cref="UserDto"/>.
    /// </summary>
    /// <param name="user">Доменная сущность пользователя.</param>
    /// <returns>DTO пользователя.</returns>
    public static UserDto ToUserDto(this User user) => new UserDto
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        UserName = user.UserName,
        Email = user.Email,
    };
}