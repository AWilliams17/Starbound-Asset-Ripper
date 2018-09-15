using ApplicationUtils;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

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
            bool timedOut = false;
            await Task.Run(() =>
            {
                try
                {
                    updateAvailable = WebUtilsRelated.UpdateAvailable();
                    DispatcherTimer.Tick += DispatcherTimer_Tick;
                    DispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                    DispatcherTimer.Start();
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.Timeout)
                    {
                        DispatcherTimer.Stop();
                        timedOut = true;
                    }
                }
            });

            if (DispatcherTimer.IsEnabled)
            {
                DispatcherTimer.Stop();
            }

            if (!timedOut)
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
                else
                {
                    MessageBox.Show("No updates were found.", "No Updates available");
                }
            }
            else
            {
                MessageBox.Show("The update check timed out.", "Timed Out");
            }

            Close();
        }

        public UpdateWindow()
        {
            InitializeComponent();
            Update();
        }
    }
}
