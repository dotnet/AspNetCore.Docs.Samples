using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

using ASPNetCoreStreamingExample.SynchronousWithNewtonsoftJson.Model;
using ASPNetCoreStreamingExample.SynchronousWithNewtonsoftJson.Middleware;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System;
using System.Reflection.PortableExecutable;

namespace ASPNetCoreStreamingExample.SynchronousWithNewtonsoftJson
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Song lyrics source to be injected into instances.
            builder.Services.AddSingleton<ILyricsSource, LyricsSource>();

            var serializer = JsonSerializer.CreateDefault();

            builder.Services.AddSingleton(serializer);

            builder.Services.AddControllers().AddNewtonsoftJson();

            // Allow synchronous I/O from Newtonsoft.Json.
            builder.Services.Configure<KestrelServerOptions>(
              options =>
              {
                  options.AllowSynchronousIO = true;
              });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();

            // Register our middleware.
            app.UseSongLyrics();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            var appBuilder = (IApplicationBuilder)app;

            var hostLifetime = appBuilder.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            hostLifetime.ApplicationStarted.Register(
                () =>
                {
                    var addressesFeature = appBuilder.ServerFeatures.Get<IServerAddressesFeature>()!;

                    Console.WriteLine();
                    Console.WriteLine("Please browse to the hosted service:");

                    foreach (var address in addressesFeature.Addresses)
                        Console.WriteLine("* {0}/menu", address.Replace("0.0.0.0", "localhost"));

                    Console.WriteLine();
                });

            app.Run();
        }
    }
}
