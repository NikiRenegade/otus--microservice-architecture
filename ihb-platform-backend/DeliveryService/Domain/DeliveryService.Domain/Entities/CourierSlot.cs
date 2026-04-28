namespace DeliveryService.Domain.Entities;

public class CourierSlot
{
    public Guid Id { get; set; }
    public Guid CourierId { get; set; }
    
    public Courier Courier { get; set; }
    public DateTime TimeSlot { get; set; }
    public bool IsReserved { get; set; }
    public List<DeliveryReservation> Reservations { get; set; }
}