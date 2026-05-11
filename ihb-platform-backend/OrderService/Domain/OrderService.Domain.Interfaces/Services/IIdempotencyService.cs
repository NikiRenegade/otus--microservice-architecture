using OrderService.Domain.DTOs;

namespace OrderService.Domain.Interfaces.Services;

/// <summary>
/// Интерфейс сервиса для управления идемпотентностью операций создания заказа.
/// Использует Redis для кеширования результатов с автоматическим истечением.
/// </summary>
public interface IIdempotencyService
{
	/// <summary>
	/// Получает результат создания заказа из кеша по ключу идемпотентности.
	/// </summary>
	/// <param name="idempotencyKey">Ключ идемпотентности.</param>
	/// <returns>Результат создания заказа, если найден в кеше; иначе <c>null</c>.</returns>
	Task<OrderCreateResponseDto?> GetAsync(string idempotencyKey);

	/// <summary>
	/// Сохраняет результат создания заказа в кеш с TTL (Time To Live).
	/// </summary>
	/// <param name="idempotencyKey">Ключ идемпотентности.</param>
	/// <param name="response">Результат создания заказа для сохранения.</param>
	/// <param name="ttlHours">Время жизни кеша в часах (по умолчанию 24 часа).</param>
	Task SetAsync(string idempotencyKey, OrderCreateResponseDto response, int ttlHours = 24);
}
