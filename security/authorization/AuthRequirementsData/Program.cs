using Microsoft.AspNetCore.Authorization;
using AuthRequirementsData.Authorization;

var builder = WebApplication.CreateBuilder();

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddSingleton<IAuthorizationHandler, 
    MinimumAgeAuthorizationHandler>();

var app = builder.Build();

app.MapControllers();

app.Run();
