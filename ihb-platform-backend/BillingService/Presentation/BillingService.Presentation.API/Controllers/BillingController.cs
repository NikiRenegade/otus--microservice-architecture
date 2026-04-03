using BillingService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BillingService.Domain.Interfaces.Services;

namespace BillingService.Presentation.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class BillingController : ControllerBase
{
    private readonly IAccountService _accountService;

    public BillingController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    [Authorize]
    [HttpPost("deposit/{amount}")]
    public async Task<IActionResult> Deposit(decimal amount)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized();

        var (success, paymentId) = await _accountService.DepositAsync(Guid.Parse(userId), amount);

        if (!success)
            return BadRequest("Произошла ошибка");

        return Ok(new { paymentId = paymentId });
    }
    [Authorize]
    [HttpPost("withdraw/{amount}")]
    public async Task<IActionResult> Withdraw(decimal amount)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
            return Unauthorized();

        var (success, paymentId) = await _accountService.WithdrawAsync(Guid.Parse(userIdClaim), amount);

        if (!success)
            return BadRequest("Недостаточно средств");

        return Ok(new { paymentId = paymentId });
    }
    [Authorize(Policy = "ServiceOnly")]
    [HttpPost("internal/withdraw/{amount}")]
    public async Task<IActionResult> InternalWithdraw(decimal amount)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
            return Unauthorized();

        var userId = Guid.Parse(userIdClaim);
        var (success, paymentId) = await _accountService.WithdrawAsync(userId, amount);

        if (!success)
            return BadRequest("Недостаточно средств");

        return Ok(new { paymentId });
    }
    
    [Authorize]
    [HttpGet("account")]
    public async Task<IActionResult> Get()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userId == null)
            return Unauthorized();
        
        var account = await _accountService.GetByIdAsync(Guid.Parse(userId));
        if (account == null)
            return NotFound();

        return Ok(account);
    }
}