using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Workflow.Core.Turnstile;

namespace Web.ViewComponents;

public class TurnstileViewComponent(ITurnstileService turnstileService) : ViewComponent
{
    public IViewComponentResult Invoke(string? theme = null, string? size = null, string? callback = null)
    {
        var model = new TurnstileViewModel
        {
            SiteKey = turnstileService.SiteKey,
            Theme = theme ?? "light",
            Size = size ?? "normal",
            Callback = callback ?? "onSuccess"
        };

        return View("Default", model);
    }
}