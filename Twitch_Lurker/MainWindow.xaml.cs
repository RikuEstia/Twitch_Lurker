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
        // Holds the list of Twitch channels we’re monitoring
        private ObservableCollection<string> channels = new ObservableCollection<string>();
        
        // A timer that regularly checks if channels are live
        private System.Timers.Timer checkLiveTimer;
        
        // Where we save/load the list of channels
        private const string filePath = "channels.json";
        
        // Keeps track of the currently live channels
        private List<string> liveChannels = new List<string>();
        
        // These variables handle button visibility and app state
        public bool buttonsActive = true;
        public Visibility buttonVisibility = Visibility.Visible;
        public bool ProgramStartedBool = false;
        public bool debugKiller = false;
        
        public MainWindow()
        {
            InitializeComponent();
            
            // Set up the list of channels to show in the UI
            ChannelList.ItemsSource = channels;

            // Load any saved channels from the file
            LoadChannelsFromFile();
        }

        // This method is just for debugging—it shows live channels in the console
        private void debugger()
        {
            foreach (var channel in liveChannels)
            {
                AppendToConsole($"Debugging live channel: {channel}");
            }
        }

        // Checks the live status of each channel and manages the browser windows
        private async void CheckLiveStatus(object sender, ElapsedEventArgs e)
        {
            // Get the Twitch API token so we can check live status
            var accessToken = await TwitchAuth.GetOAuthToken();

            bool channelsOffline = false;

            // Go through each channel and see if it's live
            foreach (var channel in channels)
            {
                bool isLive = await TwitchAPI.IsChannelLive(channel, accessToken);
                if (isLive)
                {
                    AppendToConsole($"{channel} is live");

                    // Open the first live channel in the first browser window
                    if (!liveChannels.Contains(channel) && liveChannels.Count == 0)
                    {
                        liveChannels.Add(channel);
                        OpenBrowserForChannel(channel, windowNumber: 1);
                    }
                    // Open the second live channel in the second browser window
                    else if (!liveChannels.Contains(channel) && liveChannels.Count == 1)
                    {
                        liveChannels.Add(channel);
                        OpenBrowserForChannel(channel, windowNumber: 2);
                    }
                }
            }

            // Check if either of the live channels went offline
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

            // If a channel went offline, close the browsers and reset everything
            if (channelsOffline || debugKiller == true)
            {
                AppendToConsole("Closing browser windows because a channel went offline");
                if (browserProcesses[0] != null && !browserProcesses[0].HasExited)
                {
                    browserProcesses[0].Kill();
                    AppendToConsole($"Closed browser for: {liveChannels[0]}");
                }
                if (browserProcesses[1] != null && !browserProcesses[1].HasExited)
                {
                    browserProcesses[1].Kill();
                    AppendToConsole($"Closed browser for: {liveChannels[1]}");
                }

                // Reset the live channels list and check again
                liveChannels.Clear();
                AppendToConsole("No live channels left, scanning again...");

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

        // Handles the "Start Program" button click
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            buttonVisibility = Visibility.Hidden; // Hide buttons while the program is running
            ProgramStarted(); // Update the UI
            SetupTimer(); // Start the live-checking timer
            ProgramStartedBool = true; // Flag to track that the program has started
            debugger(); // Optional: Check the live channel list in the console
        }

        // Handles the "Stop Program" button click
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            buttonVisibility = Visibility.Visible; // Show buttons again
            buttonsActive = true; // Reactivate buttons
            ProgramStarted(); // Update the UI
            ProgramStartedBool = false; // Mark the program as stopped
            debugger(); // Optional: Check the live channel list in the console
        }

        // Updates the UI based on whether the program is running or stopped
        private void ProgramStarted()
        {
            // Toggle visibility of channel management buttons
            AddBtn.Visibility = buttonVisibility;
            EditBtn.Visibility = buttonVisibility;
            UpBtn.Visibility = buttonVisibility;
            RemoveBtn.Visibility = buttonVisibility;
            DownBtn.Visibility = buttonVisibility;
        }

        // Handles the "Debug Kill" button click for testing purposes
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            debugKiller = true; // Set the flag to kill browser processes
        }
    }
}
