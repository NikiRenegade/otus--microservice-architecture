namespace DeliveryService.Domain.Entities;

public class DeliveryReservation
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid CourierSlotId { get; set; }
    public CourierSlot CourierSlot { get; set; }
    public DateTime CreatedAt { get; set; }
}