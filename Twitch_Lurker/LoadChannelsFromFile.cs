namespace TwitchChannelMonitor;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows.Controls;

public partial class MainWindow
{
    private void LoadChannelsFromFile()
    {
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            channels = JsonSerializer.Deserialize<ObservableCollection<string>>(json);
            ChannelList.ItemsSource = channels;
            AppendToConsole("Channels loaded from file");
        }
        else
        {
            AppendToConsole("No channels file found");
        }
    }
}
