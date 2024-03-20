using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ViteIntegration.Sample.Models;

namespace ViteIntegration.Sample.Controllers;

[Route("")]
public class HomeController : Controller
{
    [HttpGet, Route("")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet, Route("/error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
