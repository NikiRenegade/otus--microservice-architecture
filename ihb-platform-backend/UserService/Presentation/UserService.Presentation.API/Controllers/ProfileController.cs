using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.Domain.DTOs;
using UserService.Domain.Events;
using UserService.Domain.Interfaces.Publishers;
using UserService.Domain.Interfaces.Services;

namespace UserService.Presentation.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{

    private readonly IUserService _userService;
    private readonly IUserEventPublisher _userEventPublisher;

    public ProfileController(IUserService userService, IUserEventPublisher userEventPublisher)
    {
        _userService = userService;
        _userEventPublisher = userEventPublisher;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userId == null)
            return Unauthorized();
        
        var user = await _userService.GetByIdAsync(Guid.Parse(userId));
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDto request)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            var updated = await _userService.UpdateAsync(Guid.Parse(userId), request);
            if (!updated)
            {
                return NotFound(new { message = $"Пользователь с данным Id = {userId} не найден" });
            }
            await _userEventPublisher.PublishUserUpdated(new UserUpdatedEvent() 
                { Id = Guid.Parse(userId), Email = request.Email });
            var user = await _userService.GetByIdAsync(Guid.Parse(userId));

            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка сервера при обновлении данных о пользователе", error = ex.Message });
        }
    }
}