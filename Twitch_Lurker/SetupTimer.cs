namespace TwitchChannelMonitor;

using System.Diagnostics.Eventing.Reader;
using System.Timers;
using System.Xml;

public partial class MainWindow
{
    private void SetupTimer()
    {
        if (ProgramStartedBool == false)
        {
            AppendToConsole("Program Started");
            checkLiveTimer = new Timer(15000); // Check every 15 seconds
            checkLiveTimer.Elapsed += CheckLiveStatus;
            checkLiveTimer.Start();
        }
    }
}
