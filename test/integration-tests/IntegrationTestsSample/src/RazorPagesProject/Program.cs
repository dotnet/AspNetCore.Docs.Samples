using System.Data.Common;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RazorPagesProject.Data;
using RazorPagesProject.Services;

var appBuilder = WebApplication.CreateBuilder(args);
var services = appBuilder.Services;

// Create open SqliteConnection so EF won't automatically close it.
services.AddSingleton<DbConnection>(container =>
{
    var connection = new SqliteConnection("DataSource=:memory:");
    connection.Open();

    return connection;
});

services.AddDbContext<ApplicationDbContext>((container, options) =>
{
    var connection = container.GetRequiredService<DbConnection>();
    options.UseSqlite(connection);
});

services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// <snippet1>
services.AddRazorPages(options =>
{
    options.Conventions.AuthorizePage("/SecurePage");
});
// </snippet1>

services.AddHttpClient<IGithubClient, GithubClient>(client =>
{
    client.BaseAddress = new Uri("https://api.github.com");
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Yolo", "0.1.0"));
});

// <snippet2>
services.AddScoped<IQuoteService, QuoteService>();
// </snippet2>

services.AddDatabaseDeveloperPageExceptionFilter();

using var app = appBuilder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var container = scope.ServiceProvider;
    var db = container.GetRequiredService<ApplicationDbContext>();

    db.Database.EnsureCreated();

    if (!db.Messages.Any())
    {
        try
        {
            db.Initialize();
        }
        catch (Exception ex)
        {
            var logger = container.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred seeding the database. Error: {Message}", ex.Message);
        }
    }
}

app.Run();

public partial class Program { }
