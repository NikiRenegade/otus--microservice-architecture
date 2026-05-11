using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Domain.DTOs;
using OrderService.Domain.Interfaces.Services;

namespace OrderService.Presentation.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(IList<OrderItemDto> orderItems, DateTime timeSlot)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
            return Unauthorized();
        
        Request.Headers.TryGetValue("request-Id", out var requestIdValue);
        var requestId = requestIdValue.ToString();

        var orderCreateDto = new OrderCreateDto
        {
            UserId = Guid.Parse(userIdClaim),
            Items = orderItems.ToList(),
            TimeSlot = timeSlot,
            IdempotencyKey = requestId
        };

        var order = await _orderService.CreateOrder(orderCreateDto);

        return Ok(order);
    }
}