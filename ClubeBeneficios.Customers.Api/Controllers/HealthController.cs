using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClubeBeneficios.Customers.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Get()
    {
        return Ok(new
        {
            service = "ClubeBeneficios.Customers.Api",
            status = "ok",
            utcNow = DateTime.UtcNow
        });
    }
}