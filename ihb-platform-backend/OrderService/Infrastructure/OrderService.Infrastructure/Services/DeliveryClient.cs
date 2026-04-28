using System.Net.Http.Headers;
using System.Net.Http.Json;
using OrderService.Domain.Interfaces.Services;
using Shared.ServiceToken.Interfaces;

namespace OrderService.Infrastructure.Services;

public class DeliveryClient : IDeliveryClient
{
    private readonly HttpClient _httpClient;
    private readonly IServiceTokenGenerator _tokenGenerator;

    public DeliveryClient(HttpClient httpClient, IServiceTokenGenerator tokenGenerator)
    {
        _httpClient = httpClient;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<bool> Reserve(Guid orderId, Guid userId, DateTime timeSlot)
    {
        var token = _tokenGenerator.Generate(
            "order-service", userId,
            "internal", "delivery.internal");
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/delivery/reserve");
        request.Content = JsonContent.Create(new
        {
            OrderId = orderId,
            TimeSlot = timeSlot
        });
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.SendAsync(request);

        return response.IsSuccessStatusCode;
    }

    public async Task Cancel(Guid orderId, Guid userId)
    {
        var token = _tokenGenerator.Generate(
            "order-service", userId,
            "internal", "delivery.internal");

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"/api/delivery/cancel/{orderId}");

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        await _httpClient.SendAsync(request);
        
    }
}