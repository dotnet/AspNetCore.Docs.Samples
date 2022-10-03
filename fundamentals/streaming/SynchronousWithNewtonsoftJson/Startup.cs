using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

using ASPNetCoreStreamingExample.SynchronousWithNewtonsoftJson.Middleware;
using ASPNetCoreStreamingExample.SynchronousWithNewtonsoftJson.Model;

namespace ASPNetCoreStreamingExample.SynchronousWithNewtonsoftJson
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Song lyrics source to be injected into instances.
            services.AddSingleton<ILyricsSource, LyricsSource>();

            var serializer = JsonSerializer.CreateDefault();

            services.AddSingleton(serializer);

            services.AddControllers().AddNewtonsoftJson();

            // Allow synchronous I/O from Newtonsoft.Json.
            services.Configure<KestrelServerOptions>(
              options =>
              {
                  options.AllowSynchronousIO = true;
              });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            // Register our middleware.
            app.UseSongLyrics();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
