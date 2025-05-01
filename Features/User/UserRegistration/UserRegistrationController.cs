using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZealotZone.Features.User.UserRegistration;

[Area("User")]
public class UserRegistrationController : Controller
{
    [HttpGet]
    [AllowAnonymous]
    [Route("/user-registration")]
    public IActionResult Index()
    {
        return View();
    }
}
