using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebMinRouteGroup;
using WebMinRouteGroup.Data;
using WebMinRouteGroup.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = "http://localhost:5000",
        ValidAudience = "http://localhost:5000",
        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String("Efgwc3/0vEGk/+eS0HP0Hw==")),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization(o => o.AddPolicy("AdminsOnly",
                                  b => b.RequireClaim("admin", "true")));

builder.Services.AddTransient<ITodoService, TodoService>();
builder.Services.AddSingleton<IEmailService, EmailService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TodoGroupDbContext>(options =>
{
    var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    options.UseSqlite($"Data Source={Path.Join(path, "WebMinRouteGroup.db")}");
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetService<TodoGroupDbContext>();
db?.Database.MigrateAsync();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");

app.MapGet("/token",  (HttpContext context) =>
{
    var username = context.Request.Headers["username"].ToString();
    var password = context.Request.Headers["password"].ToString();
    
    if (username.Equals("admin") && password.Equals("admin"))
    {
        var issuer = "http://localhost:5000";
        var audience = "http://localhost:5000";
        var securityKey = new SymmetricSecurityKey(Convert.FromBase64String("Efgwc3/0vEGk/+eS0HP0Hw=="));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = audience,
            Issuer = issuer,
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddMinutes(2),
            Subject = new ClaimsIdentity(new []
            {
                new Claim("admin", "true")
            })
        };

        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var token = jwtTokenHandler.CreateToken(tokenDescriptor);

        return Results.Ok(new
        {
            access_token = jwtTokenHandler.WriteToken(token)
        });
    }

    return Results.Unauthorized();
}).AllowAnonymous();

app.MapGet("/admin", () => "Authorized Endpoint")
    .RequireAuthorization("AdminsOnly");

// todoV1 endpoints
app.MapGroup("/todos/v1")
    .MapTodosApiV1()
    .WithTags("Todo Endpoints");

// todoV2 endpoints
app.MapGroup("/todos/v2")
    .MapTodosApiV2()
    .WithTags("Todo Endpoints");

app.Run();

public partial class Program
{ }
