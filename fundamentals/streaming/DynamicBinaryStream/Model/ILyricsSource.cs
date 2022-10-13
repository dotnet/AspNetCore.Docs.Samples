namespace DynamicBinaryStream.Model;

public interface ILyricsSource
{
    IAsyncEnumerable<string> GetSongLyrics();
}
