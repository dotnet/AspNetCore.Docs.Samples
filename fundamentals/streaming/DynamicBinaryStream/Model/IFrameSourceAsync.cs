using System.Collections.Generic;
using System.Drawing;

namespace ASPNetCoreStreamingExample.DynamicBinaryStream.Model
{
    public interface IFrameSourceAsync
    {
        IAsyncEnumerable<Bitmap> GetFrames();
    }
}