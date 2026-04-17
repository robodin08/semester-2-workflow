namespace Workflow.Core.Turnstile;

public interface ITurnstileService
{
    Task<TurnstileResponse> VerifyTokenAsync(string token, string? remoteip = null);
    string SiteKey { get; }
}