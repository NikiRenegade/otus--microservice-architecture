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
    private readonly IInventoryClient _inventory;
    private readonly IDeliveryClient _delivery;
    private readonly IOrderEventPublisher _orderEventPublisher;
    private readonly IIdempotencyService _idempotencyService;

    public OrderService(
        IOrderRepository orderRepository,
        IBillingClient billing,
        IInventoryClient inventory,
        IDeliveryClient delivery,
        IOrderEventPublisher orderEventPublisher,
        IIdempotencyService idempotencyService)
    {
        _orderRepository = orderRepository;
        _billing = billing;
        _inventory = inventory;
        _delivery = delivery;
        _orderEventPublisher = orderEventPublisher;
        _idempotencyService = idempotencyService;
    }

    public async Task<OrderCreateResponseDto> CreateOrder(OrderCreateDto orderCreateDto)
    {
        if (!string.IsNullOrEmpty(orderCreateDto.IdempotencyKey))
        {
            var cachedResult = await _idempotencyService.GetAsync(orderCreateDto.IdempotencyKey);
            if (cachedResult != null)
            {
                return cachedResult;
            }
        }
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

        var reserved = await _inventory.Reserve(order.Id, orderCreateDto.UserId, orderCreateDto.Items);

        if (!reserved)
        {
            await Fail(order, "Нет товара на складе");
            orderCreateResponseDto = Map(order);
            await CacheSetAsync(orderCreateDto.IdempotencyKey, orderCreateResponseDto);
            return orderCreateResponseDto;
        }

        var deliveryReserved = await _delivery.Reserve(order.Id, orderCreateDto.UserId, orderCreateDto.TimeSlot);

        if (!deliveryReserved)
        {
            await _inventory.Release(order.Id, orderCreateDto.UserId);
            await Fail(order, "Нет доступного курьера");
            orderCreateResponseDto = Map(order);
            await CacheSetAsync(orderCreateDto.IdempotencyKey, orderCreateResponseDto);
            return orderCreateResponseDto;
        }

        var payment = await _billing.Withdraw(order.UserId, order.Price);

        if (!payment.Success)
        {
            await _orderRepository.UpdatePaymentAndStatusAsync(order.Id, payment.PaymentId, OrderStatus.Failed);
            await _inventory.Release(order.Id, orderCreateDto.UserId);
            await _delivery.Cancel(order.Id, orderCreateDto.UserId);
            await Fail(order, "Оплата не прошла");

            orderCreateResponseDto = new OrderCreateResponseDto
            {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                PaymentId = order.PaymentId,
                Price = order.Price,
                UserId = order.UserId,
                Status = order.Status.ToString(),
            };
            await CacheSetAsync(orderCreateDto.IdempotencyKey, orderCreateResponseDto);
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

        await CacheSetAsync(orderCreateDto.IdempotencyKey, orderCreateResponseDto);
        return orderCreateResponseDto;
    }

    private async Task Fail(Order order, string message)
    {
        await _orderRepository.UpdatePaymentAndStatusAsync(
            order.Id,
            null,
            OrderStatus.Failed
        );

        await _orderEventPublisher.PublishOrderCompleted(new OrderСompletedEvent
        {
            UserId = order.UserId,
            Text = $"Заказ {order.Id} создать не удалось. " + message
        });
    }

    private OrderCreateResponseDto Map(Order order)
    {
        return new OrderCreateResponseDto
        {
            Id = order.Id,
            CreatedAt = order.CreatedAt,
            PaymentId = order.PaymentId,
            Price = order.Price,
            UserId = order.UserId,
            Status = order.Status.ToString()
        };
    }
    
    private async Task CacheSetAsync(string? idempotencyKey, OrderCreateResponseDto response)
    {
        if (!string.IsNullOrEmpty(idempotencyKey))
        {
            await _idempotencyService.SetAsync(idempotencyKey, response);
        }
    }
}