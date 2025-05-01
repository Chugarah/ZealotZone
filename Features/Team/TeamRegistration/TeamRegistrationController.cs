using Microsoft.AspNetCore.Mvc;
using ZealotZone.Features.TeamRegistration.ViewModels;

namespace ZealotZone.Features.TeamRegistration;

public class TeamRegistrationController : Controller
{
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
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        return Ok(new { Message = "Testing :)" });
    }
}