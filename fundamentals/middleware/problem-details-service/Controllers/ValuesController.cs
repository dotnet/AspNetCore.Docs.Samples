using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace ProblemDetailsWebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ValuesController : ControllerBase
{
    [HttpGet("{numerator}/{denominator}")]
    public IActionResult divide(double numerator, double denominator)
    {
        if (denominator == 0)
        {
            HttpContext.Features.GetRequiredFeature<MathErrorFeature>().MathError =
                                                         MathErrorType.DivisionByZeroError;
            return BadRequest();
        }

        var calculation = numerator / denominator;
        return Ok(calculation);
    }

    [HttpGet("{radicand}")]
    public IActionResult squareroot(double radicand)
    {
        if (radicand < 0)
        {
            HttpContext.Features.GetRequiredFeature<MathErrorFeature>().MathError =
                                                           MathErrorType.NegativeRadicandError;
            return BadRequest();
        }

        var calculation = Math.Sqrt(radicand);
        return Ok(calculation);
    }

}
