using System.Collections.Generic;

namespace ASPNetCoreStreamingExample.AsynchronousWithSystemTextJson.Model
{
  public interface ILyricsSource
  {
    IAsyncEnumerable<string> GetSongLyrics();
  }
}
