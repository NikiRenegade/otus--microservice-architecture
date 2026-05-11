namespace DeliveryService.Domain.DTOs;

public class ReserveDeliveryDto
{
    public Guid OrderId { get; set; }
    public DateTime TimeSlot { get; set; }
}