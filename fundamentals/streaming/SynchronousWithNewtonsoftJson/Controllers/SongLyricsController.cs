using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using ASPNetCoreStreamingExample.SynchronousWithNewtonsoftJson.Model;
using ASPNetCoreStreamingExample.SynchronousWithNewtonsoftJson.Results;

namespace ASPNetCoreStreamingExample.SynchronousWithNewtonsoftJson.Controllers
{
    [Route("/v1")]
    public class SongLyricsController : Controller
    {
        ILyricsSource _lyricsSource;
        JsonSerializer _serializer;

        public SongLyricsController(ILyricsSource lyricsSource, JsonSerializer serializer)
        {
            _lyricsSource = lyricsSource;
            _serializer = serializer;
        }

        [HttpGet("sing")]
        public IActionResult PerformSong()
        {
            return new SongLyricsResult(_lyricsSource, _serializer);
        }
    }
}
