namespace TwitchChannelMonitor;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http.Headers;

public class TwitchAPI
{
    private static readonly string clientId = "j94kca2edntj336kvnqf6mr7l29tx5"; // Make sure this is set
    private static readonly string baseUrl = "https://api.twitch.tv/helix/streams";

    public static async Task<bool> IsChannelLive(string channelName, string accessToken)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Client-Id", clientId);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync($"{baseUrl}?user_login={channelName}");
            var json = await response.Content.ReadAsStringAsync();

            var jsonData = JsonDocument.Parse(json);
            if (jsonData.RootElement.TryGetProperty("data", out JsonElement data))
            {
                return data.GetArrayLength() > 0; // If data is not empty, channel is live
            }

            return false; // Channel is not live
        }
    }
}
