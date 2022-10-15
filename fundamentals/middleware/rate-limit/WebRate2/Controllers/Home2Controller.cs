using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Diagnostics;
using WebMVCrm.Models;

namespace WebMVCrm.Controllers;
// <snippet_1>
[EnableRateLimiting("fixed")]
public class Home2Controller : Controller
{
    private readonly ILogger<Home2Controller> _logger;

    public Home2Controller(ILogger<Home2Controller> logger)
    {
        _logger = logger;
    }

    public ActionResult Index()
    {
        return View();
    }

    [EnableRateLimiting("sliding")]
    public ActionResult Privacy()
    {
        return View();
    }

    [DisableRateLimiting]
    public ActionResult NoLimit()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
// </snippet_1>
