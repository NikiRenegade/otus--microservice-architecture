using OrderService.Domain.DTOs;

namespace OrderService.Domain.Interfaces.Services;

public interface IOrderService
{
    public Task<OrderCreateResponseDto> CreateOrder(OrderCreateDto orderCreateDto);
}