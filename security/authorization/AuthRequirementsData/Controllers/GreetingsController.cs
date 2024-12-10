using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthRequirementsData.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GreetingsController : Controller
{
    [MinimumAgeAuthorize(16)]
    [HttpGet("hello")]
    public string Hello() => $"Hello {(HttpContext.User.Identity?.Name ?? "world")}!";

    [Authorize(Policy = "MinimumAge16")]
    [HttpGet("helloPolicy")]
    public string HelloPolicy() => $"helloPolicy {(HttpContext.User.Identity?.Name ?? "world")}!";
}
