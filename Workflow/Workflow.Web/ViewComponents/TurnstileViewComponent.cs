using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Workflow.Core.Turnstile;

namespace Web.ViewComponents;

public class TurnstileViewComponent(ITurnstileService turnstileService) : ViewComponent
{
    public IViewComponentResult Invoke(string? theme, string? size)
    {
        var model = new TurnstileViewModel(
            siteKey: turnstileService.SiteKey,
            theme: theme ?? "light",
            size: size ?? "normal",
            onTurnstileSuccess: "onTurnstileSuccess",
            onTurnstileError: "onTurnstileError",
            onTurnstileExpired: "onTurnstileExpired"
        );

        return View("Default", model);
    }
}