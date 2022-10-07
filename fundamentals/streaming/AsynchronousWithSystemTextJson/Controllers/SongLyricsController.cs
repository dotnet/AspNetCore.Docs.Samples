namespace AsynchronousWithSystemTextJson.Controllers;

using Microsoft.AspNetCore.Mvc;

using AsynchronousWithSystemTextJson.Model;

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
