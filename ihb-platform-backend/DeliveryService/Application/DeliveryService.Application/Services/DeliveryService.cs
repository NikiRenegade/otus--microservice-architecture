using DeliveryService.Domain.DTOs;
using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Interfaces.Repositories;
using DeliveryService.Domain.Interfaces.Services;

namespace DeliveryService.Application.Services;

public class DeliveryService : IDeliveryService
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeliveryService(IDeliveryRepository deliveryRepository, IUnitOfWork unitOfWork)
    {
        _deliveryRepository = deliveryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Reserve(ReserveDeliveryDto dto)
    {
        if (await _deliveryRepository.HasDeliveryReservation(dto.OrderId))
            return true;

        var slot = await _deliveryRepository.GetFreeCourierSlot(dto.TimeSlot);

        if (slot == null)
            return false;

        slot.IsReserved = true;

        await _deliveryRepository.AddDeliveryReservation(new DeliveryReservation
        {
            Id = Guid.NewGuid(),
            OrderId = dto.OrderId,
            CourierSlotId = slot.Id,
            CreatedAt = DateTime.UtcNow
        });

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task Cancel(Guid orderId)
    {
        var reservation = await _deliveryRepository.GetDeliveryReservation(orderId);

        if (reservation == null)
            return;

        var slot = await _deliveryRepository.GetCourierSlot(reservation.CourierSlotId);

        slot.IsReserved = false;

        await _deliveryRepository.RemoveDeliveryReservation(reservation);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<CourierDto> CreateCourier(CreateCourierDto dto)
    {
        var courier = new Courier
        {
            Id = Guid.NewGuid(),
            Name = dto.Name
        };

        await _deliveryRepository.AddCourier(courier);
        await _unitOfWork.SaveChangesAsync();

        return new CourierDto { Id = courier.Id, Name = courier.Name };
    }

    public async Task<CourierSlotDto> CreateSlot(CreateCourierSlotDto dto)
    {
        var slot = new CourierSlot
        {
            Id = Guid.NewGuid(),
            CourierId = dto.CourierId,
            TimeSlot = dto.TimeSlot.ToUniversalTime(),
            IsReserved = false
        };

        await _deliveryRepository.AddCourierSlot(slot);
        await _unitOfWork.SaveChangesAsync();
        return new CourierSlotDto
        {
            Id = slot.Id,
            CourierId = slot.CourierId,
            TimeSlot = slot.TimeSlot,
            IsReserved = slot.IsReserved
            
        };
    }

    public async Task<List<CourierSlotDto>> GetSlots()
    {
        var slots = await _deliveryRepository.GetAllSlots();
        return slots.Select(x=> new CourierSlotDto
        {
            Id = x.Id,
            CourierId = x.CourierId,
            TimeSlot = x.TimeSlot,
            IsReserved = x.IsReserved,
            Courier = new CourierDto { Id = x.Courier.Id, Name = x.Courier.Name }
        }).ToList();
    }
}