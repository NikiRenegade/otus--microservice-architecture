using DeliveryService.Domain.DTOs;
using DeliveryService.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.Presentation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeliveryController : ControllerBase
{
    private readonly IDeliveryService _deliveryService;
    
    public DeliveryController(IDeliveryService deliveryService)
    {
        _deliveryService = deliveryService;
    }
    [Authorize(Policy = "ServiceOnly")]
    [HttpPost("reserve")]
    public async Task<IActionResult> Reserve(ReserveDeliveryDto dto)
    {
        var result = await _deliveryService.Reserve(dto);

        if (!result)
            return BadRequest("No available courier");

        return Ok(result);
    }
    [Authorize(Policy = "ServiceOnly")]
    [HttpPost("cancel/{orderId}")]
    public async Task<IActionResult> Cancel(Guid orderId)
    {
        await _deliveryService.Cancel(orderId);
        return Ok();
    }
    
    [HttpPost("courier")]
    public async Task<IActionResult> CreateCourier(CreateCourierDto dto)
    {
        var courier = await _deliveryService.CreateCourier(dto);
        return Ok(courier);
    }

    [HttpPost("slot")]
    public async Task<IActionResult> CreateSlot(CreateCourierSlotDto dto)
    {
        var slot = await _deliveryService.CreateSlot(dto);
        return Ok(slot);
    }

    [HttpGet("slots")]
    public async Task<IActionResult> GetSlots()
    {
        var slots = await _deliveryService.GetSlots();
        return Ok(slots);
    }
    [HttpGet("slot/{slotId}")]
    public async Task<IActionResult> GetSlotById(Guid slotId)
    {
        var slot = await _deliveryService.GetCourierSlotById(slotId);
        return Ok(slot);
    }
}