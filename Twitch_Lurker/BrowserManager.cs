namespace Twitch_Lurker
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using System;

    public class BrowserManager
    {
        private IWebDriver chromeDriver;
        private string currentChannelUrl;

        public BrowserManager()
        {
            var options = new ChromeOptions();
            options.AddArgument("--mute-audio"); // Optional if you want to mute initially
            chromeDriver = new ChromeDriver(options);
            chromeDriver.Manage().Window.Maximize();
        }

        public void OpenStream(string channelUrl)
        {
            currentChannelUrl = channelUrl;
            chromeDriver.Navigate().GoToUrl(channelUrl);
            AdjustVolumeAndQuality();
        }

        private void AdjustVolumeAndQuality()
        {
            // Cast IWebDriver to IJavaScriptExecutor
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)chromeDriver;

            // Set volume to 1%
            string setVolumeScript = "document.querySelector('video').volume = 0.01;";
            jsExecutor.ExecuteScript(setVolumeScript);

            // Set video quality to 160p or 360p if 160p is unavailable
            string setQualityScript = @"
    var settingsButton = document.querySelector('button[data-a-target=""player-settings-button""]');
    if (settingsButton) {
        settingsButton.click();
        var qualityOptions = Array.from(document.querySelectorAll('div')).filter(el => el.textContent.includes('160p') || el.textContent.includes('360p'));
        if (qualityOptions.length > 0) {
            qualityOptions[0].click(); // Select the lowest available quality
        }
    }";
            jsExecutor.ExecuteScript(setQualityScript);
        }


        public bool IsBrowserOpen()
        {
            try
            {
                return chromeDriver.WindowHandles.Count > 0;
            }
            catch (WebDriverException)
            {
                return false;
            }
        }

        public void CloseBrowser()
        {
            chromeDriver.Quit();
        }

        public bool IsOnSameChannel(string url)
        {
            return currentChannelUrl.Equals(url, StringComparison.OrdinalIgnoreCase);
        }
    }
}
