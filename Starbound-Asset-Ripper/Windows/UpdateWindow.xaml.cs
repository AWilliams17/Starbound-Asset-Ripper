using SharpUtils.WebUtils;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Starbound_Asset_Ripper.Windows
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow
    {
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        DispatcherTimer DispatcherTimer = new DispatcherTimer();
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            TimeoutProgressBar.Value += 1;
        }

        private async void Update()
        {
            try
            {
                bool updateAvailable = false;

                DispatcherTimer.Tick += DispatcherTimer_Tick;
                DispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                DispatcherTimer.Start();
                
                updateAvailable = await GithubReleaseParser.GetUpdateAvailableAsync("AWilliams17", "Starbound-Asset-Ripper", 5, cancellationTokenSource.Token);

                DispatcherTimer.Stop();
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
            catch (WebException ex)
            {
                DispatcherTimer.Stop(); // So that when an error message comes up the timer still isn't ticking.
                if (ex.Status == WebExceptionStatus.Timeout)
                {
                    MessageBox.Show("Check Timed Out after 5 seconds", "Check Timed Out");
                }
                else
                {
                    MessageBox.Show($"Error occurred while checking for updates: {ex.Message}", "Error checking for updates");
                }
            }
            Close();
        }

        public UpdateWindow()
        {
            InitializeComponent();
            Update();
        }

        private void UpdateWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cancellationTokenSource.Cancel();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
