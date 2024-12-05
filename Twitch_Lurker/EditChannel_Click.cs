namespace TwitchChannelMonitor;
using System.Windows;

public partial class MainWindow
{
    private void EditChannel_Click(object sender, RoutedEventArgs e)
    {
        if (ChannelList.SelectedItem != null && buttonsActive == true)
        {
            string selectedChannel = ChannelList.SelectedItem.ToString();
            string updatedChannel = Microsoft.VisualBasic.Interaction.InputBox("Edit Twitch channel name:", "Edit Channel", selectedChannel);
            if (!string.IsNullOrEmpty(updatedChannel) && updatedChannel != selectedChannel)
            {
                AppendToConsole($"Editing channel: {selectedChannel} to {updatedChannel}");
                int index = channels.IndexOf(selectedChannel);
                channels[index] = updatedChannel;
                SaveChannelsToFile();
            }
            else
            {
                AppendToConsole("No changes made to the channel");
            }
        }
        else
        {
            AppendToConsole("No channel selected for editing");
        }
    }
}
