using ASPNetCoreStreamingExample.DynamicBinaryStream.Model;
using ASPNetCoreStreamingExample.DynamicBinaryStream.Results;

using Microsoft.AspNetCore.Mvc;

namespace ASPNetCoreStreamingExample.DynamicBinaryStream.Controllers
{
    public class AnimationController
    {
        IFrameSourceAsync _frameSource;

        public AnimationController(IFrameSourceAsync frameSource)
        {
            _frameSource = frameSource;
        }

        [HttpGet("/v1/sing")]
        public IActionResult ProduceAnimation()
        {
            return new AnimationResult(_frameSource.GetFrames());
        }
    }
}
