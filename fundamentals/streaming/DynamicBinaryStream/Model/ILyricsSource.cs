using System.Collections.Generic;

namespace ASPNetCoreStreamingExample.DynamicBinaryStream.Model
{
    public interface ILyricsSource
    {
        IAsyncEnumerable<string> GetSongLyrics();
    }
}
