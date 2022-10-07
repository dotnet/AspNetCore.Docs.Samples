namespace SynchronousWithNewtonsoftJson.Model;

public interface ILyricsSource
{
    IEnumerable<string> GetSongLyrics();
}
