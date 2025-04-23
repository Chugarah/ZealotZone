using Microsoft.AspNetCore.Mvc;

namespace ZealotZone.Controllers;

[Route("[controller]")]
public class ProjectsController : Controller
{
    // GET
    public IActionResult Projects()
    {
        return View();
    }
}