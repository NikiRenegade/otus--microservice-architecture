using System.Net.Http.Headers;
using System.Net.Http.Json;
using OrderService.Domain.DTOs;
using OrderService.Domain.Interfaces.Services;
using Shared.ServiceToken.Interfaces;

namespace OrderService.Infrastructure.Services;

public class BillingClient : IBillingClient
{
    private readonly HttpClient _httpClient;
    private readonly IServiceTokenGenerator _tokenGenerator;

    public BillingClient(HttpClient httpClient, IServiceTokenGenerator tokenGenerator)
    {
        _httpClient = httpClient;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<(bool Success, Guid? PaymentId)> Withdraw(Guid userId, decimal amount)
    {
        var token = _tokenGenerator.Generate(
            "order-service", userId,
            "internal", "billing.internal");

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"api/billing/internal/withdraw/{amount}");

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return (false, null);

        var result = await response.Content.ReadFromJsonAsync<PaymentResponseDto>();

        return (true, result?.PaymentId);
    }
    
}
    