using System.Text.Json.Serialization;

namespace Workflow.Core.Turnstile;

public class TurnstileResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; init; }

    [JsonPropertyName("challenge_ts")]
    public DateTime? ChallengeTs { get; init; }

    [JsonPropertyName("hostname")]
    public string? Hostname { get; init; }

    [JsonPropertyName("error-codes")]
    public List<string>? ErrorCodes { get; init; }

    [JsonPropertyName("action")]
    public string? Action { get; init; }

    [JsonPropertyName("cdata")]
    public string? CData { get; init; }
}