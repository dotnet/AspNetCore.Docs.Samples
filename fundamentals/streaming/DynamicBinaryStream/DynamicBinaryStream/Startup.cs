using System;

using ASPNetCoreStreamingExample.DynamicBinaryStream.Model;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASPNetCoreStreamingExample.DynamicBinaryStream
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

            // Frame generator for animation
            services.AddSingleton<IFrameSourceAsync, FrameSourceAsync>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            var addressesFeature = app.ServerFeatures.Get<IServerAddressesFeature>()!;

            Console.WriteLine();
            Console.WriteLine("Please browse to the hosted service:");

            foreach (var address in addressesFeature.Addresses)
                Console.WriteLine("* {0}/menu", address.Replace("0.0.0.0", "localhost"));

            Console.WriteLine();
        }
    }
}
