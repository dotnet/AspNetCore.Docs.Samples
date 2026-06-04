using Contoso.API;
using Contoso.API.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient(AppConstants.SecurityAPIClient, httpClient =>
{
    httpClient.BaseAddress = new Uri("https://localhost:7123/");
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddSingleton<IAuthorizationHandler, CanGetWeatherAuthorizationHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AppConstants.CanGetWeatherPolicyName, policy =>
        policy.Requirements.Add(new CanGetWeatherRequirement()));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
