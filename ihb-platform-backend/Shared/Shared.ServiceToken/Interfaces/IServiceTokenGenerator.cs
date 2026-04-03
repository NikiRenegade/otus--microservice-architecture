namespace Shared.ServiceToken.Interfaces;

public interface IServiceTokenGenerator
{
    string Generate(string serviceName, Guid userId, string audience, string scope);
}