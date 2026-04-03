using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.Domain.DTOs;
using UserService.Domain.Events;
using UserService.Domain.Interfaces.Publishers;
using UserService.Domain.Interfaces.Services;

namespace UserService.Presentation.API.Controllers;

/// <summary>
/// Контроллер для управления профилем текущего аутентифицированного пользователя.
/// Предоставляет endpoints для получения и обновления личных данных профиля.
/// Требует JWT аутентификацию.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    /// <summary>
    /// Сервис для работы с пользователями.
    /// </summary>
    private readonly IUserService _userService;
    
    /// <summary>
    /// Издатель событий пользователя для публикации изменений.
    /// </summary>
    private readonly IUserEventPublisher _userEventPublisher;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ProfileController"/>.
    /// </summary>
    /// <param name="userService">Сервис для работы с пользователями.</param>
    /// <param name="userEventPublisher">Издатель событий пользователя.</param>
    public ProfileController(IUserService userService, IUserEventPublisher userEventPublisher)
    {
        _userService = userService;
        _userEventPublisher = userEventPublisher;
    }

    /// <summary>
    /// Получает профиль текущего аутентифицированного пользователя.
    /// </summary>
    /// <returns>OK (200) с данными профиля пользователя, или NotFound (404), если пользователь не найден.</returns>
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

    /// <summary>
    /// Обновляет профиль текущего аутентифицированного пользователя.
    /// Публикует событие обновления e-mail для других микросервисов.
    /// </summary>
    /// <param name="request">DTO с новыми данными профиля пользователя.</param>
    /// <returns>OK (200) с обновленными данными пользователя, или NotFound (404), если пользователь не найден.</returns>
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