internal class AppSettings
{
    public Miner[] Miners { get; set; }

    public int SwitchLag { get; set; }
}

internal class Miner
{
    public string Ip { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public Pool[] Pools { get; set; }
}

internal class Pool
{
    public string Url { get; set; }
    public string User { get; set; }
    public string Pass { get; set; }
    public string Algo { get; set; }
}