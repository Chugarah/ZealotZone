using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using ZealotZone.Features.TeamRegistration.ViewModels;

namespace ZealotZone.Features.TeamRegistration.ViewComponents;

public class AddTeamMemberViewComponent(IAntiforgery antiforgery) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(AddMemberViewModel? model = null)
    {
        model ??= new AddMemberViewModel();

        // Get the antiforgery token
        var antiforgeryToken = antiforgery.GetAndStoreTokens(HttpContext).RequestToken;
        ViewData["AntiForgeryToken"] = antiforgeryToken;

        // Return the view with the model
        return await Task.FromResult(View(model));
    }
}
