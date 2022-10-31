using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

internal class GoldshellMcbClient
{
    private HttpClient Client = new();
    private string Username = "";
    private string Password = "";

    internal GoldshellMcbClient(string baseUrl, string username, string password)
    {
        Client.DefaultRequestHeaders.Accept.Clear();
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

        Client.BaseAddress = new Uri(baseUrl);
        Username = username;
        Password = password;
        Client.Timeout = TimeSpan.FromSeconds(10);
    }

    internal async Task<AlgoSettingResponse> GetAlgo()
    {
        return await Get<AlgoSettingResponse>("mcb/algosetting");
    }

    internal async Task<PoolResponse[]> GetPools()
    {
        return await Get<PoolResponse[]>("mcb/pools");
    }

    internal async Task<CgminerDevsResponse> GetHashrate()
    {
        return await Get<CgminerDevsResponse>("mcb/cgminer?cgminercmd=devs");
    }

    internal async Task<AddPoolResponse[]> AddPool(AddPoolRequest addPoolRequest)
    {
        var content = new StringContent(JsonSerializer.Serialize<AddPoolRequest>(addPoolRequest), Encoding.UTF8, "application/json");
        return await Put<AddPoolResponse[]>("mcb/newpool", content);
    }

    internal async Task<HttpStatusCode> SetAlgo(string algo, AlgoSettingResponse algoSettings)
    {
        var setAlgoRequest = new SetAlgoRequest(algoSettings, algo);

        var content = new StringContent(JsonSerializer.Serialize<SetAlgoRequest>(setAlgoRequest), Encoding.UTF8, "application/json");
        return await Put("mcb/algosetting", content);
    }

    internal async Task<DelPoolResponse[]> DelPool(DelPoolRequest delPoolRequest)
    {
        var content = new StringContent(JsonSerializer.Serialize<DelPoolRequest>(delPoolRequest), Encoding.UTF8, "application/json");
        return await Put<DelPoolResponse[]>("mcb/delpool", content);
    }

    internal async Task<HttpStatusCode> Restart()
    {
        return await Put("mcb/restart");
    }

    private async Task<T> Get<T>(string urlPart)
    {
        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };


        try
        {
            var response = await Client.GetAsync(urlPart);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Put: {Client.BaseAddress}{urlPart} | Response: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                return default;
            }

            return await JsonSerializer.DeserializeAsync<T>(response.Content.ReadAsStream(), options);
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine($"Http timeout, check if miner is accessible at: {Client.BaseAddress}{urlPart}");
            return default;
        }
    }

    private async Task<T> Put<T>(string urlPart, HttpContent content)
    {
        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        try
        {
            var response = await Client.PutAsync(urlPart, content);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await Authenticate(Username, Password);
                response = await Client.PutAsync(urlPart, content);
            }

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Put: {Client.BaseAddress}{urlPart} | Response: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                return default;
            }

            return await JsonSerializer.DeserializeAsync<T>(response.Content.ReadAsStream(), options);
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Unresponsive, rebooting... ");
            await Restart();
            Console.WriteLine("Wait until miner is backup... ");
            while (!(await Client.GetAsync("")).IsSuccessStatusCode) { }

            var response = await Client.PutAsync(urlPart, content);
            return await JsonSerializer.DeserializeAsync<T>(response.Content.ReadAsStream(), options);
        }
    }

    private async Task<HttpStatusCode> Put(string urlPart, HttpContent content)
    {
        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        try
        {
            var response = await Client.PutAsync(urlPart, content);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await Authenticate(Username, Password);
                response = await Client.PutAsync(urlPart, content);
            }

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Put: {Client.BaseAddress}{urlPart} | Response: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            }

            return response.StatusCode;
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Unresponsive, rebooting... ");
            await Restart();
            Console.WriteLine("Wait until miner is backup... ");
            while (!(await Client.GetAsync("")).IsSuccessStatusCode) { }
            var response = await Client.PutAsync(urlPart, content);
            return response.StatusCode;
        }
    }

    private async Task<HttpStatusCode> Put(string urlPart)
    {
        try
        {
            var response = await Client.PutAsync(urlPart, null);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await Authenticate(Username, Password);
                response = await Client.PutAsync(urlPart, null);
            }

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Put: {Client.BaseAddress}{urlPart} | Response: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            }

            return response.StatusCode;
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Unresponsive, rebooting... ");
            await Restart();
            Console.WriteLine("Wait until miner is backup... ");
            while (!(await Client.GetAsync("")).IsSuccessStatusCode)
            {

            }

            var response = await Client.PutAsync(urlPart, null);
            return response.StatusCode;
        }
    }

    private async Task Authenticate(string username, string password)
    {
        var response = await Client.GetAsync($"user/login?username={username}&password={password}");
        var unlockResponse = await JsonSerializer.DeserializeAsync<UnlockResponse>(response.Content.ReadAsStream());

        Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {unlockResponse.JwtToken}");
    }
}