using System;
using System.Windows;
using System.Windows.Media;
using Starbound_Asset_Ripper.ConfigContainer;
using ApplicationUtils;
using Microsoft.Win32;

namespace Starbound_Asset_Ripper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private bool steamPathSet = false;
        private bool outputPathSet = false;
        private bool workshopPathSet = false;
        public static Config config = new Config();

        public MainWindow()
        {
            InitializeComponent();
            ResizeMode = 0;
            Closing += MainWindow_Closing;
            MiscUtils.WarnIfNotAdmin();
            
            string loadSettingsResult = config.settings.LoadSettings();
            if (loadSettingsResult != null || !config.settings.RootKeyExists())
            {
                // TODO: Should be a message box here.
                MessageBox.Show(loadSettingsResult);
                config.settings.SaveSettings();
            }
            steamPathSet = (config.settings.GetOption<string>("SteamPath") != "");
            workshopPathSet = (config.settings.GetOption<string>("WorkshopPath") != "");
            outputPathSet = (config.settings.GetOption<string>("OutputPath") != "");

            UpdateControls();
        }

        private void OutputPathBtn_Click(object sender, RoutedEventArgs e)
        {
            string outputPath = MiscUtils.SelectFolderDialog("");
            if (outputPath != null)
            {
                config.settings.SetOption("OutputPath", outputPath);
                config.settings.SaveSettings();
                outputPathSet = true;
                UpdateControls();
            }
        }

        private void SteamPathBtn_Click(object sender, RoutedEventArgs e)
        {
            string steamPath = MiscUtils.SelectFolderDialog(@"Browse to the steam installation EG: C:\Program Files(x86)\Steam");
            if (steamPath != null)
            {
                string workshopPath = MiscUtils.GetWorkShopPath(steamPath);

                if (workshopPath == null)
                {
                    string errorMessage = $"The Starbound Workshop folder was not found in the selected Steam install." +
                        $"{Environment.NewLine}It should be located at 'Steam\\steamapps\\workshop\\content\\211820'.{Environment.NewLine}" +
                        $"{Environment.NewLine}Do you have any mods installed?";
                    MessageBox.Show(errorMessage, "Couldn't find Starbound Workshop folder");
                }
                else
                {
                    workshopPathSet = true;
                    config.settings.SetOption("WorkshopPath", workshopPath);
                }
                config.settings.SetOption("SteamPath", steamPath);
                config.settings.SaveSettings();
                steamPathSet = true;
                UpdateControls();
            }
        }

        private void HelpBtn_Click(object sender, RoutedEventArgs e)
        {
            WebUtilsRelated.OpenGithubPage();
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

        private void LoadPaks()
        {

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

        private void SetLabels()
        {
            void SetLabelContent(string LabelText, Brush StatusColor)
            {
                StatusLabel.Content = LabelText;
                StatusLabel.Foreground = StatusColor;
            }

            if (steamPathSet && outputPathSet)
            {
                if (!workshopPathSet)
                {
                    SetLabelContent("Starbound workshop folder not found in Steam folder.", LabelColors.Bad);
                }
                else
                {
                    UnpackAllBtn.IsEnabled = true;
                    UnpackSelectedBtn.IsEnabled = true;
                    SetLabelContent("Everything looks good.", LabelColors.Good);
                }
            }
            else
            {
                SetLabelContent("Waiting for paths to be set...", LabelColors.Bad);
            }

        }

        private void SetTextBoxes()
        {
            SteamPathTextBox.Text = "Path to Steam installation...";
            OutputPathTextBox.Text = "Path to output folder...";

            if (steamPathSet)
            {
                SteamPathTextBox.Text = config.settings.GetOption<string>("SteamPath");
            }
            if (outputPathSet)
            {
                OutputPathTextBox.Text = config.settings.GetOption<string>("OutputPath");
            }
        }

        private void UpdateControls()
        {
            SetTextBoxes();
            SetLabels();
            if (steamPathSet && workshopPathSet && outputPathSet)
            {
                LoadPaks();
            }
        }
    }
}
