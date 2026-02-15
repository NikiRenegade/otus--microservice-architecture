namespace UserService.Domain.DTOs;

 public record UserUpdateDto
(
     string Email,
     string UserName,
     string FirstName,
     string LastName
);