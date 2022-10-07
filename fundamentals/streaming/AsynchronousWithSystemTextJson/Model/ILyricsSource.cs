namespace AsynchronousWithSystemTextJson.Model;

public interface ILyricsSource
{
    IAsyncEnumerable<string> GetSongLyrics();
}
