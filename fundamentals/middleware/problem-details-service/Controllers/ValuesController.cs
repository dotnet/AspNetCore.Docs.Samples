using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace ProblemDetailsWebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ValuesController : ControllerBase
{
    // /api/values/1/2
    [HttpGet("{Numerator}/{Denominator}")]
    public IActionResult Divide(double Numerator, double Denominator)
    {
        if (Denominator == 0)
        {
            HttpContext.Features.GetRequiredFeature<MathErrorFeature>().MathError =
                                                         MathErrorType.DivisionByZeroError;
            return BadRequest();
        }

        var calculation = Numerator / Denominator;
        return Ok(calculation);
    }

    [HttpGet("{radicand}")]
    public IActionResult Squareroot(double radicand)
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
