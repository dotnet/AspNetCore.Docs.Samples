namespace DynamicBinaryStream.Model;

using System.Drawing;

public class FrameSourceAsync : IFrameSourceAsync
{
    const int FrameWidth = 1000;
    const int FrameHeight = 64;

    ILyricsSource _lyricsSource;
    Font _font;

    public FrameSourceAsync(ILyricsSource lyricsSource)
    {
        _lyricsSource = lyricsSource;
        _font = new Font("Consolas", FrameHeight / 3);
    }

    public async IAsyncEnumerable<Bitmap> GetFrames()
    {
        var startTime = DateTime.UtcNow;

        await foreach (var lyricLine in _lyricsSource.GetSongLyrics())
        {
            yield return GetFrame(lyricLine, DateTime.UtcNow - startTime);
        }
    }

    public Bitmap GetFrame(string lyricLine, TimeSpan frameTime)
    {
        var bitmap = new Bitmap(FrameWidth, FrameHeight);

        using (var graphics = Graphics.FromImage(bitmap))
        using (var backgroundBrush = new SolidBrush(Color.White))
        {
            graphics.FillRectangle(backgroundBrush, x: 0, y: 0, width: FrameWidth, height: FrameHeight);

            graphics.DrawString(lyricLine, _font, Brushes.Black, x: 10, y: 0);
            graphics.DrawString(frameTime.ToString(), _font, Brushes.Gray, x: 10, y: FrameHeight / 2);
        }

        return bitmap;
    }
}
