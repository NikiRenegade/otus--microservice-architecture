using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Shared.ServiceToken.Interfaces;

namespace Shared.ServiceToken;

/// <summary>
/// Генератор JWT токенов для межсервисной коммуникации.
/// Создает подписанные токены с указанными претензиями (claims) для аутентификации между микросервисами.
/// </summary>
public class ServiceTokenGenerator : IServiceTokenGenerator
{
    /// <summary>
    /// Конфигурация приложения для получения JWT параметров (ключ, издатель).
    /// </summary>
    private readonly IConfiguration _config;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ServiceTokenGenerator"/>.
    /// </summary>
    /// <param name="config">Конфигурация приложения.</param>
    public ServiceTokenGenerator(IConfiguration config)
    {
        _config = config;
    }

    /// <summary>
    /// Генерирует подписанный JWT токен для межсервисного взаимодействия.
    /// Токен содержит информацию о сервисе, пользователе и разрешениях. Срок действия - 5 минут.
    /// </summary>
    /// <param name="serviceName">Имя сервиса, создающего токен (будет в поле 'sub').</param>
    /// <param name="userId">ID пользователя для включения в токен.</param>
    /// <param name="audience">Целевая аудитория.</param>
    /// <param name="scope">Область действия токена (API разрешения).</param>
    /// <returns>Строка подписанного JWT токена.</returns>
    public string Generate(string serviceName, Guid userId, string audience, string scope)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim("sub", serviceName),
            new Claim("type", "service"),
            new Claim("scope", scope)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"])
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(5),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}