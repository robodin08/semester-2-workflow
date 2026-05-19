using Microsoft.AspNetCore.Mvc;
using Web.Filters;

namespace Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}