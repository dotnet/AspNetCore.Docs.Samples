#define RDG13 //   RDG13 RDG13F
#if NEVER
#elif RDG13
// <snippet_1>
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0,
                                 AppJsonSerializerContext.Default);
});
builder.Services.AddKeyedSingleton<IService, FizzService>("fizz");

var app = builder.Build();

app.MapGet("/fizz", ([FromKeyedServices("fizz")][FromServices] IService service) =>
{
    return Results.Ok(service.Echo());
});

app.Run();
// </snippet_1>
public interface IService
{
    string Echo();
}

public class FizzService : IService
{
    public string Echo()
    {
        return "Fizz";
    }
}
[JsonSerializable(typeof(string[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
#elif RDG13F
// <snippet_1f>
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0,
                                 AppJsonSerializerContext.Default);
});

builder.Services.AddKeyedSingleton<IService, FizzService>("fizz");
builder.Services.AddKeyedSingleton<IService, BuzzService>("buzz");
builder.Services.AddSingleton<IService, FizzBuzzService>();
var app = builder.Build();

app.MapGet("/fizz", ([FromKeyedServices("fizz")] IService service) =>
{
    return Results.Ok(service.Echo());
}); 

app.MapGet("/buzz", ([FromKeyedServices("buzz")] IService service) =>
{
    return Results.Ok(service.Echo());
}); 

app.MapGet("/fizzbuzz", ([FromServices] IService service) =>
{
    return Results.Ok(service.Echo());
});

app.Run();

public interface IService
{
    string Echo();
}

public class FizzService : IService
{
    public string Echo() => "Fizz";
}

public class BuzzService : IService
{
    public string Echo() => "Buzz";
}

public class FizzBuzzService : IService
{
    public string Echo()
    {
        return "FizzBuzz";
    }
}
[JsonSerializable(typeof(string[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
// </snippet_1f>
#endif

