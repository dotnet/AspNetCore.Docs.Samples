#define RDG13F //   RDG13 RDG13F
#if NEVER
#elif RDG13
// <snippet_1>
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKeyedSingleton<IService, FizzService>("fizz");
builder.Services.AddKeyedSingleton<IService, BuzzService>("buzz");
builder.Services.AddSingleton<IService, FizzBuzzService>();
var app = builder.Build();

app.MapGet("/fizz", ([FromKeyedServices("fizz")][FromServices] IService service) =>
{
    return Results.Ok(service.Eco());
});

app.MapGet("/buzz", ([FromKeyedServices("buzz")] IService service) =>
{
    return Results.Ok(service.Eco());
});

app.MapGet("/fizzbuzz", ([FromServices] IService service) =>
{
    return Results.Ok(service.Eco());
});

app.Run();

public interface IService
{
    string Eco();
}

public class FizzService : IService
{
    public string Eco()
    {
        return "Fizz";
    }
}

public class BuzzService : IService
{
    public string Eco()
    {
        return "Buzz";
    }
}

public class FizzBuzzService : IService
{
    public string Eco()
    {
        return "FizzBuzz";
    }
}
// </snippet_1>
#elif RDG13F
// <snippet_1f>
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKeyedSingleton<IService, FizzService>("fizz");
builder.Services.AddKeyedSingleton<IService, BuzzService>("buzz");
builder.Services.AddSingleton<IService, FizzBuzzService>();
var app = builder.Build();

app.MapGet("/fizz", ([FromKeyedServices("fizz")] IService service) =>
{
    return Results.Ok(service.Eco());
}); 

app.MapGet("/buzz", ([FromKeyedServices("buzz")] IService service) =>
{
    return Results.Ok(service.Eco());
}); 

app.MapGet("/fizzbuzz", ([FromServices] IService service) =>
{
    return Results.Ok(service.Eco());
});

app.Run();

public interface IService
{
    string Eco();
}

public class FizzService : IService
{
    public string Eco()
    {
        return "Fizz";
    }
}

public class BuzzService : IService
{
    public string Eco()
    {
        return "Buzz";
    }
}

public class FizzBuzzService : IService
{
    public string Eco()
    {
        return "FizzBuzz";
    }
}
// </snippet_1f>
#endif

