namespace Web.Models;


public class TurnstileViewModel
{
    public string SiteKey { get; init; } = null!;
    public string Callback { get; init; } = "onSuccess";
    public string Theme { get; init; } = "light";
    public string Size { get; init; } = "normal";
}