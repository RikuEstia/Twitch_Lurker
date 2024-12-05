namespace TwitchChannelMonitor;
using System.Windows;

public partial class MainWindow
{
    private void AppendToConsole(string message)
    {
        // Ensure this is done on the UI thread
        Application.Current.Dispatcher.Invoke(() =>
        {
            ConsoleTextBox.AppendText(message + System.Environment.NewLine);
            ConsoleTextBox.ScrollToEnd(); // Scroll to the bottom of the TextBox to always show the latest log
        });
    }
}