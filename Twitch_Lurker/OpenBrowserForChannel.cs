namespace TwitchChannelMonitor;
using System.Diagnostics;

public partial class MainWindow
{
    private Process[] browserProcesses = new Process[2]; // Array to store browser processes for the two live channels

    private void OpenBrowserForChannel(string channel, int windowNumber)
    {
        AppendToConsole($"Executing OpenBrowserForChannel for {channel}, window {windowNumber}");
        string url = $"https://www.twitch.tv/{channel}";

        // Define browser executable path
        string browserPath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";

        // Use command-line arguments to open the URL in a new window
        Process process = Process.Start(new ProcessStartInfo
        {
            FileName = browserPath,
            Arguments = $"--new-window {url}",
            UseShellExecute = false
        });

        // Store the process for later termination
        browserProcesses[windowNumber - 1] = process;

        AppendToConsole($"Opened browser for channel: {channel} in window {windowNumber}");
    }

}
