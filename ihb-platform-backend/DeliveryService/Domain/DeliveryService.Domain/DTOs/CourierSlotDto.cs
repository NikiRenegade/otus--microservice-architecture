namespace DeliveryService.Domain.DTOs;

public class CourierSlotDto
{
    public Guid Id { get; set; }
    public Guid CourierId { get; set; }
    
    public CourierDto Courier { get; set; }
    public DateTime TimeSlot { get; set; }
    public bool IsReserved { get; set; }
}