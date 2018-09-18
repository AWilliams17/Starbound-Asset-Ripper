using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

// TODO: This needs to be checked to make sure the Tasks are doing what they should.
// Pretty sure this is not going to fly.
namespace Starbound_Asset_Ripper.Windows
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        DispatcherTimer DispatcherTimer = new DispatcherTimer();
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            TimeoutProgressBar.Value += 1;
        }

        private async void Update()
        {
            bool updateAvailable = false;
            string checkFailureMessage = null;
            DispatcherTimer.Tick += DispatcherTimer_Tick;
            DispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            await Task.Run(() =>
            {
                try
                {
                    DispatcherTimer.Start();
                    updateAvailable = WebUtils.LatestReleaseParser.GetUpdateAvailable("AWilliams17", "Starbound-Asset-Ripper", 5);
                }
                catch (Exception ex)
                {
                    DispatcherTimer.Stop();

                    if (ex is WebException)
                    {
                        WebException webEX = (WebException)ex;
                        if (webEX.Status == WebExceptionStatus.Timeout)
                        {
                            checkFailureMessage = "The check timed out after 5 seconds.";
                        }
                    }
                    else
                    {
                        if (ex is FormatException)
                        {
                            checkFailureMessage = "Error parsing JSON. Check failed.";
                        }
                    }
                }
            });
            DispatcherTimer.Stop();

            if (checkFailureMessage == null)
            {
                if (updateAvailable)
                {
                    string updateAvailableMessage = "An update is available. Would you like to go to the download page?";
                    var userAction = MessageBox.Show(updateAvailableMessage, "Update Available", MessageBoxButton.YesNo);
                    if (userAction == MessageBoxResult.Yes)
                    {
                        Process.Start("https://github.com/AWilliams17/Starbound-Asset-Ripper/releases");
                    }
                }
                else MessageBox.Show("No updates found.", "No updates available.");
            }
            else MessageBox.Show(checkFailureMessage, "Error checking for updates");

            Close();
        }

        public UpdateWindow()
        {
            InitializeComponent();
            Update();
        }
    }
}
