using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZealotZone.Areas.Authentication.Controllers;

[Area("User")]
public class UserLoginController : Controller
{
    [HttpGet]
    [AllowAnonymous]
    [Route("/")]
    [Route("/user-login")]
    public IActionResult Index()
    {
        return View();
    }
}
