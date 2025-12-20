using Microsoft.AspNetCore.Mvc;

namespace HealthService.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { status = "OK" });
    }
}
