using System.Text.Json.Serialization;

internal class UnlockResponse
{
    [JsonPropertyName("JWT Token")]
    public string JwtToken { get; set; }
}