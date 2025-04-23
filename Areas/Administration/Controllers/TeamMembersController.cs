using Microsoft.AspNetCore.Mvc;

namespace ZealotZone.Areas.Administration.Controllers;

[Area("Administration")]
public class TeamMembersController : Controller
{
    [HttpGet]
    [Route("/team-members")]
    public IActionResult Index()
    {
        return View();
    }
}