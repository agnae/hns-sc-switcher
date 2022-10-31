using System.Text.Json.Serialization;

internal class DelPoolRequest
{
    [JsonPropertyName("url")]
    public string Url { get; set; }
    [JsonPropertyName("legal")]
    public bool Legal { get; set; }
    [JsonPropertyName("active")]
    public bool Active { get; set; }
    [JsonPropertyName("dragid")]
    public short DragId { get; set; }
    [JsonPropertyName("user")]
    public string User { get; set; }
    [JsonPropertyName("pool-priority")]
    public short PoolPriority { get; set; }
    [JsonPropertyName("pass")]
    public string Pass { get; set; }

    public DelPoolRequest(PoolResponse poolResponse)
    {
        Url = poolResponse.Url;
        Legal = poolResponse.Legal;
        Active = false;
        DragId = poolResponse.DragId;
        User = poolResponse.User;
        PoolPriority = poolResponse.PoolPriority;
        Pass = poolResponse.Pass;
    }
}