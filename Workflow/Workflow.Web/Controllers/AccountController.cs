using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Filters;
using Web.Models;
using Workflow.Core.Exceptions;
using Workflow.Core.Users;

namespace Web.Controllers;

[Authorize]
public class AccountController(
    IUserService userService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();
        var user = userService.GetUserById(userId);

        return View(new AccountViewModel
        {
            Email = user.Email,
            Username = user.Username,
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ValidateTurnstile]
    public IActionResult ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessages"] = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToArray();
            return RedirectToAction(nameof(Index));
        }

        try
        {
            userService.ChangePassword(GetUserId(), model.CurrentPassword, model.NewPassword);
            TempData["SuccessMessage"] = "Password changed successfully.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessages"] = new[]
            {
                ex is UserVisibleException
                    ? ex.Message
                    : "An unexpected error occurred.",
            };
        }

        return RedirectToAction(nameof(Index));
    }

    private int GetUserId()
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.Parse(claim ?? throw new UnauthorizedAccessException());
    }
}
