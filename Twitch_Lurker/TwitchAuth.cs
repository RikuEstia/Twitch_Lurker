namespace TwitchChannelMonitor;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

public class TwitchAuth
{
    private static readonly string clientId = "j94kca2edntj336kvnqf6mr7l29tx5";
    private static readonly string clientSecret = "qqcye795mjjires3fsewnx6615ns40";

    public static async Task<string> GetOAuthToken()
    {
        using (var client = new HttpClient())
        {
            var tokenUrl = $"https://id.twitch.tv/oauth2/token?client_id={clientId}&client_secret={clientSecret}&grant_type=client_credentials";
            var response = await client.PostAsync(tokenUrl, null);
            var json = await response.Content.ReadAsStringAsync();

            var jsonData = JsonDocument.Parse(json);
            var token = jsonData.RootElement.GetProperty("access_token").GetString();

            return token;
        }
    }
}
