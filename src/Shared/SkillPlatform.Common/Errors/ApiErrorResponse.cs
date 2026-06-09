using System.Text.Json.Serialization;

namespace SkillPlatform.Common.Errors;

public class ApiErrorResponse
{
    [JsonPropertyName("code")]
    public string Code { get; init; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; init; } = string.Empty;

    [JsonPropertyName("detail")]
    public string? Detail { get; init; }

    [JsonPropertyName("traceId")]
    public string TraceId { get; init; } = string.Empty;
}
