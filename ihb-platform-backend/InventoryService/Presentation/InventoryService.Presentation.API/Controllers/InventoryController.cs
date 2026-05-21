using InventoryService.Domain.DTOs;
using InventoryService.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Presentation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }
    
    [Authorize(Policy = "ServiceOnly")]
    [HttpPost("reserve")]
    public async Task<IActionResult> Reserve(ReserveOrderItemDto dto)
    {
        var result = await _inventoryService.Reserve(dto);

        if (!result)
            return BadRequest("Not enough stock");

        return Ok();
    }
    [Authorize(Policy = "ServiceOnly")]
    [HttpPost("release/{orderId}")]
    public async Task<IActionResult> Release(Guid orderId)
    {
        await _inventoryService.Release(orderId);
        return Ok();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddStock(AddProductStockDto dto)
    {
        var productStock = await _inventoryService.AddProductStock(dto);
        return Ok(productStock);
    }
    [Authorize]
    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProductStock(Guid productId)
    {
        var productStock = await _inventoryService.GetProductStock(productId);

        if (productStock == null)
            return NotFound();

        return Ok(productStock);
    }
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllProductStocks()
    {
        var productStock = await _inventoryService.GetAllProductStocks();

        if (productStock == null)
            return NotFound();

        return Ok(productStock);
    }
}