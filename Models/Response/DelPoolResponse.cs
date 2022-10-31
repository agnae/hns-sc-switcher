using System.Text.Json.Serialization;

internal class DelPoolResponse
{
    public string Url { get; set; }
    public bool Legal { get; set; }
    public bool Active { get; set; }
    public short DragId { get; set; }
    public string User { get; set; }
    [JsonPropertyName("pool-priority")]
    public short PoolPriority { get; set; }
    public string Pass { get; set; }
}