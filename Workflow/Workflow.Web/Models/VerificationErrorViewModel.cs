namespace Web.Models;

public class VerificationErrorViewModel
{
    public string Message { get; init; } = "We couldn't verify your request.";
    public string ReturnUrl { get; init; } = "/";
    public string ReturnButtonText { get; init; } = "Return to home";
}