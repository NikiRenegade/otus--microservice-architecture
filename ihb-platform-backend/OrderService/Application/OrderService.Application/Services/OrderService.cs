using OrderService.Domain.DTOs;
using OrderService.Domain.Entities;
using OrderService.Domain.Events;
using OrderService.Domain.Interfaces.Publishers;
using OrderService.Domain.Interfaces.Repositories;
using OrderService.Domain.Interfaces.Services;

namespace OrderService.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBillingClient _billing;
    private readonly IOrderEventPublisher _orderEventPublisher;

    public OrderService(
        IOrderRepository orderRepository,
        IBillingClient billing,
        IOrderEventPublisher orderEventPublisher)
    {
        _orderRepository = orderRepository;
        _billing = billing;
        _orderEventPublisher = orderEventPublisher;
    }

    public async Task<OrderCreateResponseDto> CreateOrder(OrderCreateDto orderCreateDto)
    {
        OrderCreateResponseDto orderCreateResponseDto;
        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = orderCreateDto.UserId,
            Status = OrderStatus.Created,
            CreatedAt = DateTime.UtcNow,
            Items = orderCreateDto.Items.Select(x => new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                Price = x.Price
            }).ToList()
        };

        order.Price = order.Items.Sum(x => x.Price * x.Quantity);

        await _orderRepository.AddAsync(order);
        
        var payment = await _billing.Withdraw(order.UserId, order.Price);

        if (!payment.Success)
        {
            await _orderRepository.UpdatePaymentAndStatusAsync(order.Id, payment.PaymentId, OrderStatus.Failed);

            await _orderEventPublisher.PublishOrderCompleted(new OrderСompletedEvent
            {
                UserId = order.UserId,
                Text = $"Оплата заказа {order.Id} не прошла"
            });
            
            orderCreateResponseDto = new OrderCreateResponseDto
            {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                PaymentId = order.PaymentId,
                Price = order.Price,
                UserId = order.UserId,
                Status = order.Status.ToString(),
            };
            return orderCreateResponseDto;
        }
        
        await _orderEventPublisher.PublishOrderCompleted(new OrderСompletedEvent
            {
                UserId = order.UserId,
                Text = $"Заказ {order.Id} успешно создан"
            });
        
        await _orderRepository.UpdatePaymentAndStatusAsync(order.Id, payment.PaymentId, OrderStatus.Completed);
        
        orderCreateResponseDto = new OrderCreateResponseDto
        {
            Id = order.Id,
            CreatedAt = order.CreatedAt,
            PaymentId = order.PaymentId,
            Price = order.Price,
            UserId = order.UserId,
            Status = order.Status.ToString(),
        };
        
        return orderCreateResponseDto;
    }
}