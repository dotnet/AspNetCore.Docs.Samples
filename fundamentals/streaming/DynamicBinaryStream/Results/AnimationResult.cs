using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace ASPNetCoreStreamingExample.DynamicBinaryStream.Results
{
    public class AnimationResult : IActionResult
    {
        IAsyncEnumerable<Bitmap> _frames;

        public AnimationResult(IAsyncEnumerable<Bitmap> frames)
        {
            _frames = frames;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.ContentType = "image/gif";

            using (var encoder = new AnimatedGif.AnimatedGifCreator(context.HttpContext.Response.Body, delay: 20))
            {
                await foreach (var frame in _frames.WithCancellation(context.HttpContext.RequestAborted))
                {
                    using (frame)
                    {
                        await encoder.AddFrameAsync(frame, cancellationToken: context.HttpContext.RequestAborted);
                    }
                }
            }
        }
    }
}
