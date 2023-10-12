using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKeyedSingleton<ICache, BigCache>("big");
builder.Services.AddKeyedSingleton<ICache, SmallCache>("small");
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();

public interface ICache
{
    object Get(string key);
}
public class BigCache : ICache
{
    public object Get(string key) => $"Resolving {key} from big cache.";
}

public class SmallCache : ICache
{
    public object Get(string key) => $"Resolving {key} from small cache.";
}

[ApiController]
[Route("/cache")]
public class CustomServicesApiController : Controller
{
    public ActionResult<object> GetOk(ICache cache)
    {
        return cache.Get("data-mvc");
    }

    [HttpGet("big")]
    public ActionResult<object> GetBigCache([FromKeyedServices("big")] ICache cache)
    {
        return GetOk(cache);
    }

    [HttpGet("small")]
    public ActionResult<object> GetSmallCache([FromKeyedServices("small")] ICache cache)
    {
        return GetOk(cache);
    }
}
