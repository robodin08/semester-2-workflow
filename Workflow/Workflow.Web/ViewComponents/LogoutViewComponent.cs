using Microsoft.AspNetCore.Mvc;

namespace Web.ViewComponents;

public class LogoutViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}