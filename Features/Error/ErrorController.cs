using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZealotZone.Features.Error;

public class ErrorController : Controller
{
    [HttpGet]
    [AllowAnonymous]
    [Route("/error")]
    public IActionResult Index()
    {
        return View();
    }
}