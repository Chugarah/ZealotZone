using Microsoft.AspNetCore.Mvc;

namespace ZealotZone.Features.Team.DisplayTeam;

public class DisplayTeamController : Controller
{
    [HttpGet]
    [Route("/show-team-members")]
    public IActionResult Index()
    {
        return View();
    }
}