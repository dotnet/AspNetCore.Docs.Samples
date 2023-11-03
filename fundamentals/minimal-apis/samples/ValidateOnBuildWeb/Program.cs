#define FIRST // FIRST SECOND ValidateOnBuild
#if NEVER
#elif FIRST
/* An easy way to test Dev vs release is to set 
 *  "ASPNETCORE_ENVIRONMENT": "Release" in one profile in launchSettings.json
 */
// <snippet_1>
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<MyScopedService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Development environment");
}
else
{
    Console.WriteLine("Release environment");
}

app.MapGet("/", context =>
{
    // Intentionally getting service provider from app, not from the request
    // This causes an exception from attempting to resolve a scoped service
    // outside of a scope.
    // Throws System.InvalidOperationException:
    // 'Cannot resolve scoped service 'MyScopedService' from root provider.'
    var service = app.Services.GetRequiredService<MyScopedService>();
    return context.Response.WriteAsync("Service resolved");
});

app.Run();

public class MyScopedService { }
// </snippet_1>
#elif SECOND
// <snippet_2>
var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    Console.WriteLine("Development environment");
    // Doesn't detect the validation problems because ValidateScopes is false.
    builder.Host.UseDefaultServiceProvider(options =>
    {
        options.ValidateScopes = false;
        options.ValidateOnBuild = false;
    });
}
// </snippet_2>
else
{
    Console.WriteLine("Release environment");
}

builder.Services.AddScoped<MyScopedService>();

var app = builder.Build();

app.MapGet("/", context =>
{
    // Intentionally getting service provider from app, not from the request
    // This causes an exception from attempting to resolve a scoped service
    // outside of a scope.
    // Throws System.InvalidOperationException:
    // 'Cannot resolve scoped service 'MyScopedService' from root provider.'
    var service = app.Services.GetRequiredService<MyScopedService>();
    return context.Response.WriteAsync("Service resolved");
});

app.Run();

public class MyScopedService { }

#elif ValidateOnBuild
// <snippet_vob>
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<MyScopedService>();
builder.Services.AddScoped<AnotherService>();

// System.AggregateException: 'Some services are not able to be constructed (Error
// while validating the service descriptor 'ServiceType: AnotherService Lifetime:
// Scoped ImplementationType: AnotherService': Unable to resolve service for type
// 'BrokenService' while attempting to activate 'AnotherService'.)'
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Development environment");
}
else
{
    Console.WriteLine("Release environment");
}

app.MapGet("/", context =>
{
    var service = context.RequestServices.GetRequiredService<MyScopedService>();
    return context.Response.WriteAsync("Service resolved correctly!");
});

app.Run();

public class MyScopedService { }

public class AnotherService
{
    public AnotherService(BrokenService brokenService) { }
}

public class BrokenService { }

// </snippet_vob>
#endif
