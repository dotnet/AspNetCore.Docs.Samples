using Microsoft.AspNetCore.Authorization;
using Contoso.API;
using Contoso.API.Authorization;
using NSwag;
using NSwag.Generation.Processors.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient(AppConstants.SecurityAPIClient, httpClient =>
{
    httpClient.BaseAddress = new Uri("https://localhost:7123/");
});

builder.Services.AddAuthentication().AddJwtBearer();

builder.Services.AddSingleton<IAuthorizationHandler, 
    CanGetWeatherAuthorizationHandler>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AppConstants.CanGetWeatherPolicyName, policy =>
        policy.Requirements.Add(new CanGetWeatherRequirement()));

// In ASP.NET Core 6.0 or earlier, use the following API to add the
// authorization policy instead of the preceding code:
/*
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AppConstants.CanGetWeatherPolicyName, policy =>
        policy.Requirements.Add(new CanGetWeatherRequirement()));
});
*/

// Add Endpoints API Explorer
builder.Services.AddEndpointsApiExplorer();

// Add NSwag services with JWT Bearer authentication
builder.Services.AddOpenApiDocument(options =>
{
    options.Title = "Contoso API";
    options.Version = "v1";

    // Add JWT Bearer security definition
    options.AddSecurity("Bearer", new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Enter your JWT token in the text input below."
    });

    // Apply security globally to all operations
    options.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Add OpenAPI/Swagger generator and the Swagger UI
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/WeatherForecast", (ILogger<Program> logger) =>
{
    var forecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = summaries[Random.Shared.Next(summaries.Length)]
    })
    .ToArray();

    return forecast;
})
.WithName("GetWeatherForecast")
.RequireAuthorization(AppConstants.CanGetWeatherPolicyName);

app.Run();
