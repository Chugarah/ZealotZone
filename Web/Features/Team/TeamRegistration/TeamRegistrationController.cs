using Microsoft.AspNetCore.Mvc;
using ZealotZone.Features.Team.TeamRegistration.ViewModels;

namespace ZealotZone.Features.Team.TeamRegistration;

public class TeamRegistrationController : Controller
{
    [Area("Team")]
    [HttpGet]
    [Route("/add-member")]
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca5391?source=recommendations
    /// </summary>
    /// <param name="addMemberModel"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/add-member")]
    public IActionResult AddMember([FromForm] AddMemberViewModel addMemberModel)
    {
        return !ModelState.IsValid ? ValidationProblem(ModelState)
            : Ok(new { Message = "Testing :)" });
    }
}