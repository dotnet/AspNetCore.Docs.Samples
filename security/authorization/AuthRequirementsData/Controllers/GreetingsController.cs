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
    public string Hello(ClaimsPrincipal user) => $"Hello {(user.Identity?.Name ?? "world")}!";
}
