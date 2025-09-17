using Microsoft.AspNetCore.Mvc;

namespace InternshipRecords.Web.Controllers;

[Route("api/health")]
[ApiController]
public class HealthController : ControllerBase
{
    [HttpPost]
    public IActionResult Alive()
    {
        return Ok();
    }

    [HttpPost("ready")]
    public IActionResult Ready()
    {
        return Ok();
    }
}