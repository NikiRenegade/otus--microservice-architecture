using System.Text.Json;
using OrderService.Domain.DTOs;
using OrderService.Domain.Interfaces.Services;
using StackExchange.Redis;

namespace OrderService.Infrastructure.Services;

/// <summary>
/// Реализация сервиса идемпотентности, использующая Redis для кеширования.
/// </summary>
public class IdempotencyService : IIdempotencyService
{
	private readonly IDatabase _redisDb;
	private const string KeyPrefix = "order:";

	public IdempotencyService(IConnectionMultiplexer redis)
	{
		_redisDb = redis.GetDatabase();
	}

	public async Task<OrderCreateResponseDto?> GetAsync(string idempotencyKey)
	{
		if (string.IsNullOrWhiteSpace(idempotencyKey))
			return null;

		var key = $"{KeyPrefix}{idempotencyKey}";
		var value = await _redisDb.StringGetAsync(key);

		if (!value.HasValue)
			return null;

		return JsonSerializer.Deserialize<OrderCreateResponseDto>(value.ToString());
	}

	public async Task SetAsync(string idempotencyKey, OrderCreateResponseDto response, int ttlHours = 24)
	{
		if (string.IsNullOrWhiteSpace(idempotencyKey))
			return;

		var key = $"{KeyPrefix}{idempotencyKey}";
		var json = JsonSerializer.Serialize(response);
		var expiry = TimeSpan.FromHours(ttlHours);

		await _redisDb.StringSetAsync(key, json, expiry);
	}
}
