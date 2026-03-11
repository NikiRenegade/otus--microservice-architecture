namespace UserService.Domain.DTOs;

public record UserAuthSuccess
{
    public string AccessToken { get; set; } = string.Empty;
    public UserDto UserDto { get; set; }
}