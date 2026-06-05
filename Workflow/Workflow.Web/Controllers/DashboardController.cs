using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }
}