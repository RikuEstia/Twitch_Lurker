namespace TwitchChannelMonitor;
using System.Windows;

public partial class MainWindow
{
    private void AddChannel_Click(object sender, RoutedEventArgs e)
    {
            string newChannel = Microsoft.VisualBasic.Interaction.InputBox("Enter Twitch channel name:", "Add Channel", "");
            if (!string.IsNullOrEmpty(newChannel))
            {
                AppendToConsole($"Adding new channel: {newChannel}");
                channels.Add(newChannel);
                SaveChannelsToFile();
            }
            else
            {
                AppendToConsole("No channel was added");
            }
    }
}
