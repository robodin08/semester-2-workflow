using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Web.Controllers;
using Workflow.Core.Users;

namespace Web.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
internal class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var httpContext = context.HttpContext;
        
        var userService = httpContext.RequestServices
            .GetRequiredService<IUserService>();
        
        var userId = httpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            context.Result = new RedirectToActionResult(nameof(UserController.Login), "User", null);
            
            return;
        }
        
        var user = userService.GetUserById(userId.Value);
        if (user == null)
        {
            httpContext.Session.Clear();
            
            context.Result = new RedirectToActionResult(nameof(UserController.Login), "User", null);
            
            return;
        }
        
        httpContext.Items["User"] = user;
    }
}