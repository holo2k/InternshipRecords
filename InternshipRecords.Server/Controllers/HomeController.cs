using Microsoft.AspNetCore.Mvc;

namespace InternshipRecords.Server.Controllers;

public class HomeController : ControllerBase
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
}