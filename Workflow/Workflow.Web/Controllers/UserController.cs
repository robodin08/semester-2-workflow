using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Web.Filters;
using Workflow.Core.Users;
using Web.Models;

namespace Web.Controllers;

public class UserController(IUserService userService) : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterUserViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ValidateTurnstile]
    public IActionResult Register(RegisterUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var userRequest = new CreateUserRequest(model.Email, model.Username, model.Password);
            var user = userService.CreateUser(userRequest);
            TempData["SuccessMessage"] = $"User '{user.Username}' created successfully.";
            // TODO: login
            return RedirectToAction(nameof(Login));
        }
        catch(Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}