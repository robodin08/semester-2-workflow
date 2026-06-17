namespace Web.Models;


public class TurnstileViewModel(
    string siteKey,
    string theme,
    string size,
    string onTurnstileSuccess,
    string onTurnstileError,
    string onTurnstileExpired,
    string onTurnstileTimeout)
{
    public string SiteKey { get; } = siteKey;
    public string Theme { get; } = theme;
    public string Size { get; } = size;
    public string OnTurnstileSuccess { get;} = onTurnstileSuccess;
    public string OnTurnstileError { get; } = onTurnstileError;
    public string OnTurnstileExpired { get; } = onTurnstileExpired;
    public string OnTurnstileTimeout { get; } = onTurnstileTimeout;
}