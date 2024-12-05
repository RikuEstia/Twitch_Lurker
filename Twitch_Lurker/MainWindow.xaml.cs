using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace TwitchChannelMonitor
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<string> channels = new ObservableCollection<string>();
        private System.Timers.Timer checkLiveTimer;
        private const string filePath = "channels.json";
        private List<string> liveChannels = new List<string>(); // List to store the live channels
        public bool buttonsActive = true;
        public Visibility buttonVisibility = Visibility.Visible;
        public bool ProgramStartedBool = false;
        public bool debugKiller = false;
        
        public MainWindow()
        {
            InitializeComponent();
            ChannelList.ItemsSource = channels;
            LoadChannelsFromFile();

        }
        private void debugger()
        {
            foreach (var channel in liveChannels)
            {
                AppendToConsole($"List Array / {channel}");
            }

        }
        private async void CheckLiveStatus(object sender, ElapsedEventArgs e)
        {
            var accessToken = await TwitchAuth.GetOAuthToken();

            bool channelsOffline = false;

            // Loop through each channel and check if it is live
            foreach (var channel in channels)
            {
                //AppendToConsole($"Checking if {channel} is live");
                bool isLive = await TwitchAPI.IsChannelLive(channel, accessToken);
                if (isLive)
                {
                    AppendToConsole($"{channel} is live");

                    // Open first live channel in window 1 if not already open
                    if (!liveChannels.Contains(channel) && liveChannels.Count == 0)
                    {
                        liveChannels.Add(channel);
                        OpenBrowserForChannel(channel, windowNumber: 1);
                    }
                    // Open second live channel in window 2 if not already open and different from window 1
                    else if (!liveChannels.Contains(channel) && liveChannels.Count == 1)
                    {
                        liveChannels.Add(channel);
                        OpenBrowserForChannel(channel, windowNumber: 2);
                    }
                }
            }

            // If either live channel goes offline, kill both processes and reset the list
            if (liveChannels.Count > 0 && !await TwitchAPI.IsChannelLive(liveChannels[0], accessToken))
            {
                AppendToConsole($"{liveChannels[0]} is no longer live");
                channelsOffline = true;
            }
            if (liveChannels.Count > 1 && !await TwitchAPI.IsChannelLive(liveChannels[1], accessToken))
            {
                AppendToConsole($"{liveChannels[1]} is no longer live");
                channelsOffline = true;
            }

            // If either channel is offline, kill both browser processes and reset
            if (channelsOffline || debugKiller == true)
            {
                AppendToConsole("Trying to kill process");
                if (browserProcesses[0] != null && !browserProcesses[0].HasExited)
                {
                    browserProcesses[0].Kill();
                    AppendToConsole($"Closed browser for channel: {liveChannels[0]} in window 1");
                }
                if (browserProcesses[1] != null && !browserProcesses[1].HasExited)
                {
                    browserProcesses[1].Kill();
                    AppendToConsole($"Closed browser for channel: {liveChannels[1]} in window 2");
                }

                // Clear the live channels and recheck the list
                liveChannels.Clear();
                AppendToConsole("Both live channels were offline, rechecking the list for live channels...");

                // Recheck all channels to find new live ones
                foreach (var channel in channels)
                {
                    bool isLive = await TwitchAPI.IsChannelLive(channel, accessToken);
                    if (isLive)
                    {
                        AppendToConsole($"{channel} is live");
                        if (liveChannels.Count == 0)
                        {
                            liveChannels.Add(channel);
                            OpenBrowserForChannel(channel, windowNumber: 1);
                        }
                        else if (liveChannels.Count == 1)
                        {
                            liveChannels.Add(channel);
                            OpenBrowserForChannel(channel, windowNumber: 2);
                        }
                    }
                }
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            buttonVisibility = Visibility.Hidden;
            ProgramStarted();
            SetupTimer();
            ProgramStartedBool = true;
            debugger();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            buttonVisibility = Visibility.Visible;
            buttonsActive = true;
            ProgramStarted();
            ProgramStartedBool = false;
            debugger();
        }

        private void ProgramStarted()
        {







            AddBtn.Visibility = buttonVisibility;
            EditBtn.Visibility = buttonVisibility;
            UpBtn.Visibility = buttonVisibility;
            RemoveBtn.Visibility = buttonVisibility;
            DownBtn.Visibility = buttonVisibility;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            debugKiller = true;
        }
    }

}
