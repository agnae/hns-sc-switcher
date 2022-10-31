using System.Text.Json.Serialization;

internal class AddPoolRequest
{
    [JsonPropertyName("url")]
    public string Url { get; set; }
    [JsonPropertyName("user")]
    public string User { get; set; }
    [JsonPropertyName("pass")]
    public string Pass { get; set; }

    public AddPoolRequest(Pool pool)
    {
        Url = pool.Url;
        User = pool.User;
        Pass = pool.Pass;
    }
}