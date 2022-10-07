namespace SynchronousWithNewtonsoftJson.Results;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SynchronousWithNewtonsoftJson.Model;

public class SongLyricsResult : IActionResult
{
    ILyricsSource _lyricsSource;
    JsonSerializer _serializer;

    public SongLyricsResult(ILyricsSource lyricsSource, JsonSerializer serializer)
    {
        _lyricsSource = lyricsSource;
        _serializer = serializer;
    }

    static readonly byte[] JSONArrayStart = new byte[] { (byte)'[' };
    static readonly byte[] JSONArraySeparator = new byte[] { (byte)',' };

    public async Task ExecuteResultAsync(ActionContext context)
    {
        Console.WriteLine(
          "[{0:yyyy-MM-dd HH:mm:ss}] Start streaming result to {1}",
          DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
          context.HttpContext.Connection.RemoteIpAddress);

        var startTimeUTC = DateTime.UtcNow;

        try
        {
            await context.HttpContext.Response.Body.WriteAsync(JSONArrayStart, context.HttpContext.RequestAborted);

            while (true)
            {
                foreach (string line in _lyricsSource.GetSongLyrics())
                {
                    using (var streamWriter = new StreamWriter(context.HttpContext.Response.Body))
                    using (var jsonWriter = new JsonTextWriter(streamWriter))
                        _serializer.Serialize(jsonWriter, line);

                    await context.HttpContext.Response.Body.WriteAsync(JSONArraySeparator, context.HttpContext.RequestAborted);

                    await context.HttpContext.Response.Body.FlushAsync(context.HttpContext.RequestAborted);

                    await Task.Delay(200);
                }
            }

            // No JSONArrayEnd needs to be written, because the above loop will never exit.
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            var stopTimeUTC = DateTime.UtcNow;

            var duration = stopTimeUTC - startTimeUTC;

            Console.WriteLine(
              "[{0:yyyy-MM-dd HH:mm:ss}] Stop streaming result to {1}, rickrolled for {2}",
              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
              context.HttpContext.Connection.RemoteIpAddress,
              duration);
        }
    }
}
