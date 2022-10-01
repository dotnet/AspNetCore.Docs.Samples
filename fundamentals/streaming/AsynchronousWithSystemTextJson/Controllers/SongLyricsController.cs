using Microsoft.AspNetCore.Mvc;

using ASPNetCoreStreamingExample.AsynchronousWithSystemTextJson.Model;
using System.Collections.Generic;

namespace ASPNetCoreStreamingExample.AsynchronousWithSystemTextJson.Controllers
{
  [Route("/v1")]
  public class SongLyricsController : Controller
  {
    ILyricsSource _lyricsSource;

    public SongLyricsController(ILyricsSource lyricsSource)
    {
      _lyricsSource = lyricsSource;
    }

    [HttpGet("sing")]
    public IAsyncEnumerable<string> PerformSong()
    {
      return _lyricsSource.GetSongLyrics();
    }
  }
}
