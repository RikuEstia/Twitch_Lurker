namespace TwitchChannelMonitor;
using System.Windows;

public partial class MainWindow
{
    private void RemoveChannel_Click(object sender, RoutedEventArgs e)
    {
        AppendToConsole("Executing RemoveChannel_Click");
        if (ChannelList.SelectedItem != null)
        {
            string selectedChannel = ChannelList.SelectedItem.ToString();
            AppendToConsole($"Removing channel: {selectedChannel}");
            channels.Remove(selectedChannel);
            SaveChannelsToFile();
        }
        else
        {
            AppendToConsole("No channel selected for removal");
        }
    }
}
