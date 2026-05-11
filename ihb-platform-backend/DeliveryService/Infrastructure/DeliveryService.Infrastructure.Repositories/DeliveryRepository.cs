using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Interfaces.Repositories;
using DeliveryService.Infrastructure.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Infrastructure.Repositories;

public class DeliveryRepository : IDeliveryRepository
{
    private readonly DeliveryDbContext _context;
    
    public  DeliveryRepository(DeliveryDbContext context)
    {
        _context = context;
    }

    public async Task<CourierSlot?> GetFreeCourierSlot(DateTime timeSlot)
    {
        var couriersSlots = await _context.CourierSlots.Where(x=> !x.IsReserved) .ToListAsync();
        var a = timeSlot.ToUniversalTime();
        return await _context.CourierSlots
            .Where(x => x.TimeSlot.ToUniversalTime() == timeSlot.ToUniversalTime() && !x.IsReserved)
            .FirstOrDefaultAsync();
    }

    public Task<CourierSlot?> GetCourierSlot(Guid slotId)
    {
        return _context.CourierSlots
            .Include(x => x.Courier)
            .FirstOrDefaultAsync(x=> x.Id == slotId);
    }

    public Task AddDeliveryReservation(DeliveryReservation reservation)
    {
        _context.DeliveryReservations.Add(reservation);
        return Task.CompletedTask;
    }

    public Task<DeliveryReservation?> GetDeliveryReservation(Guid orderId)
    {
        return _context.DeliveryReservations
            .FirstOrDefaultAsync(x => x.OrderId == orderId);
    }

    public Task<bool> HasDeliveryReservation(Guid orderId)
    {
        return _context.DeliveryReservations
            .AnyAsync(x => x.OrderId == orderId);
    }

    public Task RemoveDeliveryReservation(DeliveryReservation reservation)
    {
        _context.DeliveryReservations.Remove(reservation);
        return Task.CompletedTask;
    }

    public Task AddCourier(Courier courier)
    {
        _context.Couriers.Add(courier);
        return Task.CompletedTask;
    }

    public Task AddCourierSlot(CourierSlot slot)
    {
        _context.CourierSlots.Add(slot);
        return Task.CompletedTask;
    }

    public async Task<List<CourierSlot>> GetAllSlots()
    {
        return await _context.CourierSlots.Include(x => x.Courier).ToListAsync();
    }
}