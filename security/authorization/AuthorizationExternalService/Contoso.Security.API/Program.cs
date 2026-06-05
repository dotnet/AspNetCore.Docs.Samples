using Contoso.Security.API;

var builder = WebApplication.CreateBuilder(args);

// Add Endpoints API Explorer
builder.Services.AddEndpointsApiExplorer();

// Add NSwag services
builder.Services.AddOpenApiDocument();

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Add OpenAPI/Swagger generator and the Swagger UI
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/SecurityPolicy/{id}", 
    (string id, IConfiguration configuration, ILogger<Program> logger) =>
{
    var allowedClients = 
        configuration.GetSection("AllowedClients").GetChildren()
            .Select(x => x.Value).ToList();

    var dto = new SecurityPolicy
    {
        CanGetWeather = allowedClients.Contains(id)
    };

    return dto;
})
.WithName("GetSecurityPolicy");

app.Run();
