using Microsoft.Extensions.Configuration;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var algo = string.Empty;

        if (args.Length != 0)
        {
            algo = args[0].ToUpperInvariant();
        }

        if (!string.IsNullOrEmpty(algo))
        {
            Console.WriteLine($"Selected coin: {algo}");

            if (!new string[] { "HNS", "SC" }.Contains(algo))
            {
                Console.WriteLine("Coin invalid. Toggling algos");
                algo = "toggle";
            }
            else
            {
                if (algo == "SC")
                {
                    algo = "blake2b(SC)";
                }

                if (algo == "HNS")
                {
                    algo = "blake2bsha3(HNS)";
                }
                Console.WriteLine($"Switching to: {algo}");
            }
        }

        IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);

        IConfigurationRoot root = builder.Build();
        var configuration = builder.Build();

        var appSettings = new AppSettings();
        configuration.Bind(appSettings);

        foreach (var miner in appSettings.Miners)
        {
            await SwitchAlgo(miner, algo);
            Thread.Sleep(appSettings.SwitchLag);
        }
    }

    private static async Task SwitchAlgo(Miner miner, string algo)
    {
        var client = new GoldshellMcbClient($"http://{miner.Ip}/", miner.Username, miner.Password);

        var algoResponse = await client.GetAlgo();
        var poolsResponse = await client.GetPools();

        if (algoResponse == default(AlgoSettingResponse))
        {
            return;
        }

        var currentAlgo = algoResponse.Algos.First(a => a.Id == algoResponse.SelectedAlgo).Name;

        if ((algo == "toggle" || algo == "blake2b(SC)") && currentAlgo == "blake2bsha3(HNS)")
        {
            foreach (var pool in poolsResponse)
            {
                await client.DelPool(new DelPoolRequest(pool));
            }
            await client.AddPool(new AddPoolRequest(miner.Pools.First(p => p.Algo.StartsWith("blake2b(SC)"))));
            await client.SetAlgo("blake2b(SC)", algoResponse);
        }
        else if ((algo == "toggle" || algo == "blake2bsha3(HNS)") && currentAlgo == "blake2b(SC)")
        {
            foreach (var pool in poolsResponse)
            {
                await client.DelPool(new DelPoolRequest(pool));
            }
            await client.AddPool(new AddPoolRequest(miner.Pools.First(p => p.Algo.StartsWith("blake2bsha3(HNS)"))));
            await client.SetAlgo("blake2bsha3(HNS)", algoResponse);
        }
        else
        {
            var curData = await client.GetHashrate();

            foreach (var d in curData.Data)
            {
                Console.WriteLine($"{miner.Ip}[{d.Id}]: {d.Hashrate / 1000} GH/s");
            }
        }
    }
}