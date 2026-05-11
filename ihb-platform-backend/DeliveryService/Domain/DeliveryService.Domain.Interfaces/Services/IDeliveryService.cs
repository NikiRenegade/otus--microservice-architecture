using DeliveryService.Domain.DTOs;

namespace DeliveryService.Domain.Interfaces.Services;

public interface IDeliveryService
{
    Task<bool> Reserve(ReserveDeliveryDto dto);
    Task Cancel(Guid orderId);
    Task<CourierDto> CreateCourier(CreateCourierDto dto);
    Task<CourierSlotDto> CreateSlot(CreateCourierSlotDto dto);
    Task<List<CourierSlotDto>> GetSlots();
    Task<CourierSlotDto> GetCourierSlotById(Guid courierId);
    
}