using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;
using OrderService.Domain.Interfaces.Repositories;
using OrderService.Infrastructure.EntityFramework.Contexts;

namespace OrderService.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
        _context = context;
    }
    public async Task<Order?> AddAsync(Order order)
    {
        _context.Orders.Add(order);

        var result = await _context.SaveChangesAsync();
        
        if (result == 0)
        {
            return null;
        }
        return order;
    }

    public async Task<Order?> UpdatePaymentAndStatusAsync(Guid id, Guid? paymentId, OrderStatus status)
    {
        var existingOrder = await _context.Orders
            .FirstOrDefaultAsync(u => u.Id == id);
            
        if (existingOrder == null)
            return null;

        existingOrder.Status = status;
        existingOrder.PaymentId = paymentId;

        await _context.SaveChangesAsync();
        return existingOrder;
    }
}