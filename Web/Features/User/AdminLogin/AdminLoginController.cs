using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZealotZone.Areas.Authentication.Controllers;

[Area("User")]
public class AdminLoginController : Controller
{
    [HttpGet]
    [AllowAnonymous]
    [Route("/admin-login")]
    public IActionResult Index()
    {
        return View();
    }
}
