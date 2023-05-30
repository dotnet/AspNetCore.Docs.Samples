using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthRequirementsData.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GreetingsController : Controller
{
    [MinimumAgeAuthorize(16)]
    [HttpGet("hello")]
    public string Hello() => $"Hello {(HttpContext.User.Identity?.Name ?? "world")}!";

}
