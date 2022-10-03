using Microsoft.AspNetCore.Builder;

namespace ASPNetCoreStreamingExample.SynchronousWithNewtonsoftJson.Middleware
{
    public static class SongLyricsMiddlewareExtensions
    {
        public static IApplicationBuilder UseSongLyrics(this IApplicationBuilder builder)
          => builder.UseMiddleware<SongLyricsMiddleware>();
    }
}
