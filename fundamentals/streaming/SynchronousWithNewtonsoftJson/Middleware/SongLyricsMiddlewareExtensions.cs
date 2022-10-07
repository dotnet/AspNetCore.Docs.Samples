namespace SynchronousWithNewtonsoftJson.Middleware;

using Microsoft.AspNetCore.Builder;

public static class SongLyricsMiddlewareExtensions
{
    public static IApplicationBuilder UseSongLyrics(this IApplicationBuilder builder)
      => builder.UseMiddleware<SongLyricsMiddleware>();
}
