using System.Text.Json.Serialization;

internal class CgminerDevsResponse
{
    public short Status { get; set; }
    public Datum[] Data { get; set; }

    public class Datum
    {
        public long Accepted { get; set; }
        [JsonPropertyName("av_hashrate")]
        public decimal AvgHashrate { get; set; }
        public string FanSpeed { get; set; }
        public decimal Hashrate { get; set; }
        [JsonPropertyName("hwerr_ration")]
        public decimal HwErrorRatio { get; set; }
        public long HwErrors { get; set; }
        public short Id { get; set; }
        public short MinerStatus { get; set; }
        public long Nonces { get; set; }
        public short PowerPlan { get; set; }
        public long Rejected { get; set; }
        public string Temp { get; set; }
        public long Time { get; set; }
        public long Valid { get; set; }
    }
}