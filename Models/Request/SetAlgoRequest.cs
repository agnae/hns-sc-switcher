using System.Text.Json.Serialization;

internal class SetAlgoRequest
{
    [JsonPropertyName("algos")]
    public Algo[] Algos { get; set; }
    [JsonPropertyName("algo_select")]
    public short SelectedAlgo { get; set; }
    [JsonPropertyName("version")]
    public string Version { get; set; }

    public class Algo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("id")]
        public short Id { get; set; }
    }

    public SetAlgoRequest(AlgoSettingResponse response, string algoToSelect)
    {
        Algos = new Algo[response.Algos.Length];
        for (var i = 0; i < response.Algos.Length; i++)
        {
            Algos[i] = new Algo
            {
                Id = response.Algos[i].Id,
                Name = response.Algos[i].Name
            };
        }

        Version = response.Version;

        SelectedAlgo = Algos.First(a => a.Name == algoToSelect).Id;
    }
}