namespace UserService.Domain.DTOs;

public record UserRegisterDto(
    string Email,
    string Password,
    string UserName,
    string FirstName,
    string LastName
);