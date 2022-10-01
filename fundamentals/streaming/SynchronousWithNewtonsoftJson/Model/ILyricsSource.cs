using System.Collections.Generic;

namespace ASPNetCoreStreamingExample.SynchronousWithNewtonsoftJson.Model
{
  public interface ILyricsSource
  {
    IEnumerable<string> GetSongLyrics();
  }
}
