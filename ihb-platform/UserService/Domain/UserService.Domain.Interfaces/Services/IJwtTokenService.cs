using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces.Services;

/// <summary>
/// Сервис для генирации jwt
/// </summary>
public interface IJwtTokenService
{
    string GenerateJwtToken(User user);
}