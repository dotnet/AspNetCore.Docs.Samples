using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace ProblemDetailsWebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ValuesController : ControllerBase
{
    // /api/values/Divide/1/2
    [HttpGet("{Numerator}/{Denominator}")]
    public IActionResult Divide(double Numerator, double Denominator)
    {
        if (Denominator == 0)
        {
            var errorType = new MathErrorFeature { MathError = MathErrorType.DivisionByZeroError };
            HttpContext.Features.Set(errorType);
            return BadRequest();
        }

        var calculation = Numerator / Denominator;
        return Ok(calculation);
    }

    // /api/values/Squareroot
    [HttpGet("{radicand}")]
    public IActionResult Squareroot(double radicand)
    {
        if (radicand < 0)
        {
            var errorType = new MathErrorFeature { MathError = MathErrorType.NegativeRadicandError };
            HttpContext.Features.Set(errorType);
            return BadRequest();
        }

        var calculation = Math.Sqrt(radicand);
        return Ok(calculation);
    }

}
