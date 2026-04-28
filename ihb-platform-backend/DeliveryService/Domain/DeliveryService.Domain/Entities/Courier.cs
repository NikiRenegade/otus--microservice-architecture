namespace DeliveryService.Domain.Entities;

public class Courier
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public List<CourierSlot>  Slots { get; set; }
}