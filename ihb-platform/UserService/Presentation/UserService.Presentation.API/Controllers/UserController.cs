using Microsoft.AspNetCore.Mvc;
using UserService.Domain.DTOs;
using UserService.Domain.Interfaces.Repositories;
using UserService.Domain.Interfaces.Services;

namespace UserService.Presentation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
/// <summary>
/// Web API контроллер для управления пользователями.
/// Поддерживает получение списка, поиск, создание, обновление и удаление.
/// </summary>
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    /// <summary>
    /// Конструктор контроллера.
    /// </summary>
    /// <param name="userService">Сервис пользователей.</param>
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Возвращает всех пользователей.
    /// </summary>
    /// <returns>Список пользователей в виде DTO.</returns>
    [HttpGet]
    public async Task<ActionResult<IList<UserDto>>> GetAll()
    {
        try
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка сервера при получении списка пользователей", error = ex.Message });
        }
    }

    /// <summary>
    /// Возвращает пользователя по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>Пользователь в виде DTO или 404.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(Guid id)
    {
        try
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"Пользователь с данным Id = {id} не найден" });
            }
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка сервера при получении пользователя", error = ex.Message });
        }
    }

    /// <summary>
    /// Возвращает пользователя по email.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    /// <returns>Пользователь в виде DTO или 404.</returns>
    [HttpGet("email/{email}")]
    public async Task<ActionResult<UserDto>> GetByEmail(string email)
    {
        try
        {
            var user = await _userService.GetByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new { message = $"Пользователь с данным Email = {email} не найден" });
            }
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка сервера при получении пользователя", error = ex.Message });
        }
    }

    /// <summary>
    /// Создаёт нового пользователя.
    /// </summary>
    /// <param name="request">Данные регистрации пользователя.</param>
    /// <returns>HTTP 201 с данными созданного пользователя.</returns>
    [HttpPost]
    public async Task<ActionResult<UserRegisterDto>> Create([FromBody] UserRegisterDto request)
    {
        try
        {

            var result = await _userService.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка сервера при создании пользователя", error = ex.Message });
        }
    }

    /// <summary>
    /// Обновляет данные пользователя.
    /// </summary>
    /// <param name="id">Идентификатор обновляемого пользователя.</param>
    /// <param name="request">DTO с новыми данными.</param>
    /// <returns>HTTP 204 при успехе, 404 если пользователь не найден.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateDto request)
    {
        try
        {

            var updated = await _userService.UpdateAsync(id, request);
            if (!updated)
            {
                return NotFound(new { message = $"Пользователь с данным Id = {id} не найден" });
            }
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка сервера при обновлении данных о пользователе", error = ex.Message });
        }
    }

    /// <summary>
    /// Удаляет пользователя по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>HTTP 204 при успехе, 404 если не найден.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var deleted = await _userService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = $"Пользователь с данным Id = {id} не найден" });
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка сервера при удалении пользователя", error = ex.Message });
        }
    }

}