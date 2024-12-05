namespace TwitchChannelMonitor;
using System.IO;
using System.Text.Json;

public partial class MainWindow
{
    private void SaveChannelsToFile()
    {
        AppendToConsole("Executing SaveChannelsToFile");
        var json = JsonSerializer.Serialize(channels);
        File.WriteAllText(filePath, json);
        AppendToConsole("Channels saved to file");
    }
}
