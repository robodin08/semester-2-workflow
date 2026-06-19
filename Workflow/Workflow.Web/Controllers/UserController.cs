using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Filters;
using Workflow.Core.Users;
using Web.Models;
using Workflow.Core.Exceptions;

namespace Web.Controllers;

public class UserController(IUserService userService) : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Dashboard");

        return View(new LoginUserViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginUserViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var user = userService.Login(new LoginRequest(model.Email, model.Password));
            await SignInUser(user);
            return RedirectToAction("Index", "Dashboard");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty,
                ex is UserVisibleException ? ex.Message : "An unexpected error occurred. Please try again later.");

            return View(model);
        }
    }

    [HttpGet]
    // [Authorize]
    public IActionResult Register()
    {
        return View(new RegisterUserViewModel());
    }

    [HttpPost]
    // [Authorize]
    [ValidateAntiForgeryToken]
    [ValidateTurnstile]
    public async Task<IActionResult> Register(RegisterUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var user = userService.Register(new RegisterRequest(model.Email, model.Username, model.Password));
            await SignInUser(user);
            return RedirectToAction("Index", "Dashboard");
        }

        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty,
                ex is UserVisibleException ? ex.Message : "An unexpected error occurred. Please try again later.");

            return View(model);
        }
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction(nameof(Login));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private async Task SignInUser(UserResponse user, bool rememberMe = false)
    {
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = rememberMe
                });
        }
    }
}