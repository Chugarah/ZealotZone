using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZealotZone.Features.User.UserDenied;

public class UserDeniedController : Controller
{
    [HttpGet]
    [AllowAnonymous]
    [Route("/user-denied")]
    public IActionResult Index()
    {
        return View();
    }
}