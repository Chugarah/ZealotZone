using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using ZealotZone.Areas.Registrations.ViewModels;

namespace ZealotZone.Areas.Registrations.ViewComponents;

public class AddMemberViewComponent(IAntiforgery antiforgery) : ViewComponent
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
