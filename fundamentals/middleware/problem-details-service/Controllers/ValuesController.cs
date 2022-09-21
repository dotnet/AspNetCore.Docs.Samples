#define FIRST // FIRST IMPROVED
#if NEVER
#elif FIRST
// <snippet_1>
using Microsoft.AspNetCore.Mvc;

namespace ProblemDetailsWebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class Values2Controller : ControllerBase
{
    // /api/values2/divide/1/2
    [HttpGet("{Numerator}/{Denominator}")]
    public IActionResult Divide(double Numerator, double Denominator)
    {
        if (Denominator == 0)
        {
            return BadRequest();
        }

        return Ok(Numerator / Denominator);
    }

    // /api/values2 /squareroot/4
    [HttpGet("{radicand}")]
    public IActionResult Squareroot(double radicand)
    {
        if (radicand < 0)
        {
            return BadRequest();
        }

        return Ok(Math.Sqrt(radicand));
    }
}
// </snippet_1>
#elif IMPROVED
// <snippet>
using Microsoft.AspNetCore.Mvc;

namespace ProblemDetailsWebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ValuesController : ControllerBase
{
    // /api/values/divide/1/2
    [HttpGet("{Numerator}/{Denominator}")]
    public IActionResult Divide(double Numerator, double Denominator)
    {
        if (Denominator == 0)
        {
            var errorType = new MathErrorFeature
            {
                MathError =
                                                   MathErrorType.DivisionByZeroError
            };
            HttpContext.Features.Set(errorType);
            return BadRequest();
        }

        var calculation = Numerator / Denominator;
        return Ok(calculation);
    }

    // /api/values/squareroot/4
    [HttpGet("{radicand}")]
    public IActionResult Squareroot(double radicand)
    {
        if (radicand < 0)
        {
            var errorType = new MathErrorFeature
            {
                MathError =
                                                   MathErrorType.NegativeRadicandError
            };
            HttpContext.Features.Set(errorType);
            return BadRequest();
        }

        var calculation = Math.Sqrt(radicand);
        return Ok(calculation);
    }

}
// </snippet>
#endif
