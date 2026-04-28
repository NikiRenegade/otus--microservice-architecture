using System.Net.Http.Headers;
using System.Net.Http.Json;
using OrderService.Domain.DTOs;
using OrderService.Domain.Interfaces.Services;
using Shared.ServiceToken.Interfaces;

namespace OrderService.Infrastructure.Services;

public class InventoryClient : IInventoryClient
{
    private readonly HttpClient _httpClient;
    private readonly IServiceTokenGenerator _tokenGenerator;
    public InventoryClient(HttpClient httpclient, IServiceTokenGenerator tokenGenerator)
    {
        _httpClient = httpclient;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<bool> Reserve(Guid orderId, Guid userId, List<OrderItemDto> items)
    {
        
        var token = _tokenGenerator.Generate(
            "order-service", userId,
            "internal", "inventory.internal");
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/inventory/reserve");
        request.Content = JsonContent.Create(new
        {
            OrderId = orderId,
            Items = items
        });
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.SendAsync(request);

        return response.IsSuccessStatusCode;
    }

    public async Task Release(Guid orderId, Guid userId)
    {
        var token = _tokenGenerator.Generate(
            "order-service", userId,
            "internal", "inventory.internal");

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"/api/inventory/release/{orderId}");

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        await _httpClient.SendAsync(request);
    }
}