using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using Registrar;
using Starbound_Asset_Ripper.ConfigContainer;
using Web.Utils;


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
            Process.Start("https://github.com/AWilliams17/Starbound-Asset-Ripper/");
        }

        private void RedditBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UnpackSelectedBtn_Click(object sender, RoutedEventArgs e)
        {
            // Test the Readme Parser
            string parseResult = ReadmeParser.GetLineFromReadme("https://raw.githubusercontent.com/AWilliams17/DumboChat/master/README.md", "This", 5);
            MessageBox.Show(parseResult);
        }

        private void UnpackAllBtn_Click(object sender, RoutedEventArgs e)
        {
            // Test the Latest Release Parser
            string latestRelease = LatestReleaseParser.GetLatestRelease("AWilliams17", "Halo-CE-Mouse-Tool", 5);
            MessageBox.Show(latestRelease);
        }

        private void MainWindow_Closing(object sender, EventArgs e)
        {
            config.settings.SaveSettings();
        }
    }
}
