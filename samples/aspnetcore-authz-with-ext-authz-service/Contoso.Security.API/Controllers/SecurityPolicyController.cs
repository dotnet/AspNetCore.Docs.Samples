using Microsoft.AspNetCore.Mvc;

namespace Contoso.Security.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SecurityPolicyController : ControllerBase
{
    private readonly ILogger<SecurityPolicyController> _logger;
    private readonly List<string> _allowedClients;

    public SecurityPolicyController(ILogger<SecurityPolicyController> logger, IConfiguration configuration)
    {
        _logger = logger;

        _allowedClients = configuration.GetSection("AllowedClients").GetChildren().Select(x => x.Value).ToList();
    }

    [HttpGet("{id}", Name = "GetSecurityPolicy")]
    public SecurityPolicy Get(string id)
    {
        var dto = new SecurityPolicy
        {
            CanGetWeather = _allowedClients.Contains(id)
        };
        return dto;
    }
}
