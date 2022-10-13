namespace DynamicBinaryStream.Model;

using System.Drawing;

public interface IFrameSourceAsync
{
    IAsyncEnumerable<Bitmap> GetFrames();
}
