using Microsoft.AspNetCore.Mvc;

namespace ZealotZone.Features.Team.DisplayTeam;

public class DisplayTeamController : Controller
{
    [Area("Team")]
    [HttpGet]
    [Route("/show-team-members")]
    public IActionResult Index()
    {
        return View();
    }
}