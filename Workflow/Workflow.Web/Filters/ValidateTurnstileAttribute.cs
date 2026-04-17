using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Web.Models;
using Workflow.Core.Turnstile;

namespace Web.Filters;

public class ValidateTurnstileAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var httpContext = context.HttpContext;

        var turnstileService = httpContext.RequestServices
            .GetRequiredService<ITurnstileService>();

        if (!httpContext.Request.HasFormContentType)
        {
            Fail(context);
            return;
        }

        var form = await httpContext.Request.ReadFormAsync();
        var token = form["cf-turnstile-response"].FirstOrDefault();

        var remoteip = httpContext.Request.Headers["CF-Connecting-IP"].FirstOrDefault() ??
                       httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                       httpContext.Connection.RemoteIpAddress?.ToString();

        if (string.IsNullOrEmpty(token))
        {
            Fail(context);
            return;
        }

        var validation = await turnstileService.VerifyTokenAsync(token, remoteip);

        if (!validation.Success)
        {
            Fail(context);
            return;
        }

        await next();
    }
    
    private static void Fail(ActionExecutingContext context)
    {
        var request = context.HttpContext.Request;
        var url = $"{request.Path}{request.QueryString}";
        
        var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
        {
            Model = new VerificationErrorViewModel
            {
                Message = "We could not verify that you are human.",
                ReturnUrl = url,
                ReturnButtonText = "Try Again",
            }
        };

        context.Result = new ViewResult
        {
            ViewName = "VerificationError",
            ViewData = viewData,
        };
    }
}