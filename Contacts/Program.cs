using Microsoft.EntityFrameworkCore;
using Contacts.Data;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

var secretClient = new SecretClient(new Uri("https://kvorldevopsdev.vault.azure.net/"), new DefaultAzureCredential());
var secret = secretClient.GetSecretAsync("sqlconnectionstring");
var sqlConnectionString = secret.Result.Value.Value;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ModelStateErrorContext>(options =>
   options.UseSqlServer(sqlConnectionString));

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
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
