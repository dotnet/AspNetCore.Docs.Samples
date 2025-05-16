using Microsoft.AspNetCore.Mvc;
using AuthRequirementsData.Authorization;

namespace AuthRequirementsData.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GreetingsController : Controller
{
    [MinimumAgeAuthorize(21)]
    [HttpGet("hello")]
    public string Hello() => 
        $"Hello {HttpContext.User.Identity?.Name}!";
}
