using Microsoft.AspNetCore.Mvc;

namespace WebKeyedService;

[ApiController]
[Route("/cache")]
public class CustomServicesApiController : Controller
{
    // GET /cache/big-cache
    [HttpGet("big-cache")]
    public ActionResult<object> GetOk([FromKeyedServices("big")]
                                                    ICache cache)
    {
        var data = cache.Get("data-mvc");
        return cache.Get("data-mvc");
    }
}
