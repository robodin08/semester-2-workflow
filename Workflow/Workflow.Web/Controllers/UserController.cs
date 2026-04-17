using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Web.Filters;
using Workflow.Core.Users;
using Web.Models;
using Workflow.Core.Turnstile;

namespace Web.Controllers;

public class UserController(IUserService userService, ITurnstileService turnstileService) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(new RegisterUserViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ValidateTurnstile]
    public IActionResult Index(RegisterUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            // var user = userService.CreateUser(new CreateUserRequest(model.Email, model.Username, model.Password));
            // TempData["SuccessMessage"] = $"User '{user.Username}' created successfully.";n
            TempData["SuccessMessage"] = "Registration successful.";
            return RedirectToAction(nameof(Index));
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            ModelState.AddModelError(string.Empty, "Could not create user. Please try again.");
            return View(model);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}