using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZealotZone.Areas.Registrations.Controllers;

[Area("Registrations")]
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