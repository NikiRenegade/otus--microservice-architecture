using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Shared.ServiceToken.Interfaces;

namespace Shared.ServiceToken;

public class ServiceTokenGenerator : IServiceTokenGenerator
{
    
    private readonly IConfiguration _config;

    public ServiceTokenGenerator(IConfiguration config)
    {
        _config = config;
    }

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