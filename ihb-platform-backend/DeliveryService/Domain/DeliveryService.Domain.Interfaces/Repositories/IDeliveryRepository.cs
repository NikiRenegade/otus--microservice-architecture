using DeliveryService.Domain.Entities;

namespace DeliveryService.Domain.Interfaces.Repositories;

public interface IDeliveryRepository
{
    Task<CourierSlot?> GetFreeCourierSlot(DateTime timeSlot);
    Task<CourierSlot?> GetCourierSlot(Guid slotId);
    Task AddDeliveryReservation(DeliveryReservation reservation);
    Task<DeliveryReservation?> GetDeliveryReservation(Guid orderId);
    Task<bool> HasDeliveryReservation(Guid orderId);
    Task RemoveDeliveryReservation(DeliveryReservation reservation);
    Task AddCourier(Courier courier);
    Task AddCourierSlot(CourierSlot slot);
    Task<List<CourierSlot>> GetAllSlots();
}