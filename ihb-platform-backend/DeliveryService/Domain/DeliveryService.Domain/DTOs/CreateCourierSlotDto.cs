namespace DeliveryService.Domain.DTOs;

public class CreateCourierSlotDto
{
    public Guid CourierId { get; set; }
    public DateTime TimeSlot { get; set; }
}