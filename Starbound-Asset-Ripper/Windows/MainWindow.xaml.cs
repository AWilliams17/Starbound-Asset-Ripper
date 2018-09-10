using System;
using System.Windows;
using System.Windows.Media;
using Starbound_Asset_Ripper.ConfigContainer;
using ApplicationUtils;


namespace Starbound_Asset_Ripper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static Config config = new Config();

        public MainWindow()
        {
            InitializeComponent();
            ResizeMode = 0;
            Closing += MainWindow_Closing;

            string loadSettingsResult = config.settings.LoadSettings();
            if (loadSettingsResult != null || !config.settings.RootKeyExists())
            {
                // TODO: Should be a message box here.
                config.settings.SaveSettings();
            }
        }

        private void OutputPathBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SteamPathBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HelpBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void GithubBtn_Click(object sender, RoutedEventArgs e)
        {
            WebUtilsRelated.OpenGithubPage();
        }

        private void RedditBtn_Click(object sender, RoutedEventArgs e)
        {
            WebUtilsRelated.OpenRedditThread();
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            WebUtilsRelated.HandleUpdateCheck();
        }

        private void UnpackSelectedBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UnpackAllBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MainWindow_Closing(object sender, EventArgs e)
        {
            config.settings.SaveSettings();
        }

        private void ChangeLabel(string LabelText, Brush StatusColor)
        {
            StatusLabel.Content = LabelText;
            StatusLabel.Foreground = StatusColor;
        }
    }
}
