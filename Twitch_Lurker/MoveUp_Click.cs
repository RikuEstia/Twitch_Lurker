namespace TwitchChannelMonitor;
using System.Windows;

public partial class MainWindow
{
    private void MoveUp_Click(object sender, RoutedEventArgs e)
    {
        if (ChannelList.SelectedItem != null)
        {
            int selectedIndex = channels.IndexOf(ChannelList.SelectedItem.ToString());
            if (selectedIndex > 0)
            {
                string selectedChannel = channels[selectedIndex];
                AppendToConsole($"Moving channel up: {selectedChannel}");
                channels.RemoveAt(selectedIndex);
                channels.Insert(selectedIndex - 1, selectedChannel);
                ChannelList.SelectedIndex = selectedIndex - 1;
                SaveChannelsToFile();
            }
            else
            {
                AppendToConsole("Cannot move channel up");
            }
        }
    }
}
