using System.Text.Json.Serialization;

internal class AlgoSettingResponse
{
    public Algo[] Algos { get; set; }
    [JsonPropertyName("algo_select")]
    public short SelectedAlgo { get; set; }
    public string Version { get; set; }

    public class Algo
    {
        public string Name { get; set; }
        public short Id { get; set; }
    }
}