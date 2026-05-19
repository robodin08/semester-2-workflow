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
        var model = new LoginUserViewModel
        {
            Email = TempData["Email"]?.ToString() ?? string.Empty,
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ValidateTurnstile]
    public IActionResult Login(LoginUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        try
        {
            var user = userService.Login(new LoginRequest(model.Email, model.Password));
            
            HttpContext.Session.SetInt32("UserId", user.Id);
            
            return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
        }
        
        catch(Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
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
            var user = userService.Register(new RegisterRequest(model.Email, model.Username, model.Password));
            TempData["Email"] = user.Email;
            return RedirectToAction(nameof(Login));
        }
        
        catch(Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}