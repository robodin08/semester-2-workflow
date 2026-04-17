using System.Text.Json;

namespace Workflow.Core.Turnstile;

public class TurnstileService(HttpClient httpClient, string secretKey, string siteKey) : ITurnstileService
{
    private const string SiteverifyUrl = "https://challenges.cloudflare.com/turnstile/v0/siteverify";

    public string SiteKey => siteKey;
    
    public async Task<TurnstileResponse> VerifyTokenAsync(string token, string? remoteip = null)
    {
        var parameters = new Dictionary<string, string>
        {
            { "secret", secretKey },
            { "response", token }
        };
        
        if (!string.IsNullOrEmpty(remoteip))
        {
            parameters.Add("remoteip", remoteip);
        }
        
        var postContent = new FormUrlEncodedContent(parameters);
        
        try
        {
            var response = await httpClient.PostAsync(SiteverifyUrl, postContent);
            var stringContent = await response.Content.ReadAsStringAsync();

             return JsonSerializer.Deserialize<TurnstileResponse>(stringContent) ?? new TurnstileResponse{ Success = false };
        }
        catch (Exception ex)
        {
            return new TurnstileResponse{ Success = false, ErrorCodes = ["internal-error"] };
        }
    }
}