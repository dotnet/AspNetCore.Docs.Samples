namespace DynamicBinaryStream.Controllers;

using DynamicBinaryStream.Model;
using DynamicBinaryStream.Results;
using Microsoft.AspNetCore.Mvc;

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
