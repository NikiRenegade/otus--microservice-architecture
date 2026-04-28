namespace OrderService.Domain.DTOs;

/// <summary>
/// DTO для ответа о платеже.
/// </summary>
public class PaymentResponseDto
{
    /// <summary>
    /// Уникальный идентификатор платежа.
    /// </summary>
    public Guid PaymentId { get; set; }
}