using System;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RazorPagesProject.Data;
using RazorPagesProject.Services;

namespace RazorPagesProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var appBuilder = WebApplication.CreateBuilder(args);
            var services = appBuilder.Services;

            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));

            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            #region snippet1
            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizePage("/SecurePage");
            });
            #endregion

            services.AddHttpClient<IGithubClient, GithubClient>(client =>
            {
                client.BaseAddress = new Uri("https://api.github.com");
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Yolo", "0.1.0"));
            });

            #region snippet2
            services.AddScoped<IQuoteService, QuoteService>();
            #endregion

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
        }
    }
}
