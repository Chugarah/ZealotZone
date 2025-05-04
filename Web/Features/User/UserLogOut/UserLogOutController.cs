using Core.Interfaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZealotZone.Features.User.UserLogOut;

[Area("User")]
public class UserLogOutController(IUserService userService) : Controller
{
    [HttpGet]
    [AllowAnonymous]
    [Route("/user-logout")]
    public async Task<IActionResult> Index()
    {
        // Call the service to log the user out
        await userService.LogoutUserAsync();
        return RedirectToAction("Index", "UserLogin", new { area = "User" });
    }
}
