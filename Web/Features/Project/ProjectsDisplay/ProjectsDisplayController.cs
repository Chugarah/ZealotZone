using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZealotZone.Features.Project.ProjectsDisplay;

[Area("Project")]
[Authorize]
public class ProjectsDisplayController : Controller
{
    [HttpGet]
    [Route("/projects")]
    public Task<IActionResult> Projects()
    {
        return Task.FromResult<IActionResult>(View());
    }

    [HttpGet]
    [Route("/get-user-data")]
    public Task<IActionResult> UserData()
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        if (userEmail == null)
        {
            // Return Unauthorized if email is not found
            return Task.FromResult<IActionResult>(
                Unauthorized(new { Message = "User email not found." })
            );
        }

        return Task.FromResult<IActionResult>(View());
    }
}
