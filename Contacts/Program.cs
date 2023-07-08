using Microsoft.EntityFrameworkCore;
using Contacts.Data;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

const string DEV_ENVIRONMENT = "dev";

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? DEV_ENVIRONMENT;

// For some reason, I keep seeing "development" when running EF Core
if (env.ToLower() == "development")
{
    env = DEV_ENVIRONMENT;
}

var secretClient = new SecretClient(new Uri($"https://kv-orldevops-{env}.vault.azure.net/"),
    new DefaultAzureCredential());
var secret = await secretClient.GetSecretAsync("sqlconnectionstring");
var sqlConnectionString = secret.Value.Value;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ModelStateErrorContext>(options =>
   options.UseSqlServer(sqlConnectionString));

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.EnvironmentName == DEV_ENVIRONMENT)
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Contacts}/{action=Index}/{id?}");

app.Run();
