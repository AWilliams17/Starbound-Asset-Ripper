using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using Starbound_Asset_Ripper.ConfigContainer;
using ApplicationUtils;
using System.Security.Principal;
using System.Threading;
using System.ComponentModel;
using System.Threading.Tasks;

// TODO: Need to add threading around the web utils function calls
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
        private Dictionary<string, string> pakDictionary = new Dictionary<string, string>();
        private readonly BackgroundWorker unpackerWorker = new BackgroundWorker();
        private readonly BackgroundWorker updateWorker = new BackgroundWorker();
        public static Config config = new Config();

        public MainWindow()
        {
            InitializeComponent();
            ResizeMode = 0;
            Closing += MainWindow_Closing;

            if ((new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator))
            {
                MessageBox.Show("Warning - You are not running as an administrator. It is highly recommended you do so.", "Not Administrator Warning");
            }

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

            UpdateAllControls();
        }

        private void OutputPathBtn_Click(object sender, RoutedEventArgs e)
        {
            string outputPath = FileUtils.SelectFolderDialog("");
            if (outputPath != null)
            {
                config.settings.SetOption("OutputPath", outputPath);
                config.settings.SaveSettings();
                outputPathSet = true;
                UpdateAllControls();
            }
        }

        private void SteamPathBtn_Click(object sender, RoutedEventArgs e)
        {
            string steamPath = FileUtils.SelectFolderDialog(@"Browse to the steam installation EG: C:\Program Files(x86)\Steam");
            if (steamPath != null)
            {
                string workshopPath = FileUtils.GetWorkShopPath(steamPath);

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
                UpdateAllControls();
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
            string workshopPath = config.settings.GetOption<string>("WorkshopPath");
            foreach (string folder in Directory.EnumerateDirectories(workshopPath))
            {
                string folderName = folder.Substring(folder.LastIndexOf(@"\")).TrimStart('\\');
                foreach (string file in Directory.EnumerateFiles(folder))
                {
                    if (file.EndsWith(".pak"))
                    {
                        string pakDictKey = $".pak in folder: {folderName}";
                        pakDictionary.Add(pakDictKey, file);
                    }
                }
            }
        }

        private async void UnpackSelectedBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PakListBox.SelectedItem != null)
            {
                string selectedValue = PakListBox.SelectedValue.ToString();
                string selectedPakPath = pakDictionary[selectedValue];
                string steamPath = config.settings.GetOption<string>("SteamPath");
                string outputPath = config.settings.GetOption<string>("OutputPath");

                SetStatusLabel($"Unpacking {selectedValue}... This might take a moment.", LabelColors.Good);

                string[] unpackFileResult = await Task.Run(() =>
                {
                    return PakUtils.UnpackPakFile(steamPath, selectedPakPath, outputPath);
                });

                if (unpackFileResult[0] == "Error")
                {
                    MessageBox.Show($"Error occurred while unpacking {selectedValue}:{Environment.NewLine}{unpackFileResult[1]}");
                    SetStatusLabel($"Failed to unpack {selectedValue}", LabelColors.Bad);
                }
                else if (unpackFileResult[0] == "Error_EXE")
                {
                    MessageBox.Show($"Critical Error occurred while unpacking {selectedValue}:{Environment.NewLine}{unpackFileResult[1]}");
                    SetStatusLabel($"Failed to unpack: asset_unpacker.exe is missing.", LabelColors.Bad);
                }
                else
                {
                    SetStatusLabel(unpackFileResult[1], LabelColors.Good);
                }
            }
        }

        private async void UnpackAllBtn_Click(object sender, RoutedEventArgs e)
        {
            SetStatusLabel($"Unpacking all pak files... This might take a moment.", LabelColors.Good);

            DateTime operationStartTime = DateTime.Now;
            string steamPath = config.settings.GetOption<string>("SteamPath");
            string outputPath = config.settings.GetOption<string>("OutputPath");
            int itemsToProcess = PakListBox.Items.Count;
            
            string[][] unpackAllResult = await Task.Run(() =>
            {
                return PakUtils.UnpackMultiplePaks(steamPath, outputPath, PakListBox, pakDictionary);
            });
            
            string[] processedItems = unpackAllResult[0];
            string[] failedItems = unpackAllResult[1];
            string[] exeMissingError = unpackAllResult[2];
            if (exeMissingError[1] != "")
            {
                MessageBox.Show(exeMissingError[1]);
                SetStatusLabel($"Failed to unpack: asset_unpacker.exe missing", LabelColors.Bad);
            }
            else
            {
                if (failedItems[0] != null)
                {
                    MessageBox.Show($"Some files failed to unpack: {Environment.NewLine}{failedItems}");
                }
                TimeSpan timeElapsed = DateTime.Now.Subtract(operationStartTime);
                SetStatusLabel($"Unpacked {processedItems.Length}/{itemsToProcess} items in {Math.Round(timeElapsed.TotalSeconds, 4)} seconds", LabelColors.Good);
            }
        }

        private void RefreshPakListBtn_Click(Object sender, RoutedEventArgs e)
        {
            UpdatePakList();
        }

        private void MainWindow_Closing(object sender, EventArgs e)
        {
            config.settings.SaveSettings();
        }

        private void SetStatusLabel(string LabelText, Brush StatusColor)
        {
            StatusLabel.Content = LabelText;
            StatusLabel.Foreground = StatusColor;
        }

        private void UpdateLabels()
        {
            if (steamPathSet && outputPathSet)
            {
                if (!workshopPathSet)
                {
                    SetStatusLabel("Starbound workshop folder not found in Steam folder.", LabelColors.Bad);
                }
                else
                {
                    UnpackAllBtn.IsEnabled = true;
                    UnpackSelectedBtn.IsEnabled = true;
                    SetStatusLabel("Everything looks good.", LabelColors.Good);
                }
            }
            else
            {
                SetStatusLabel("Waiting for paths to be set...", LabelColors.Bad);
            }
        }

        private void UpdateTextBoxes()
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

        private void UpdatePakList()
        {
            pakDictionary.Clear();
            LoadPaks();
            foreach (KeyValuePair<string, string> kvp in pakDictionary)
            {
                PakListBox.Items.Add(kvp.Key);
            }
        }

        private void UpdateAllControls()
        {
            UpdateTextBoxes();
            UpdateLabels();
            if (steamPathSet && workshopPathSet && outputPathSet)
            {
                UpdatePakList();
            }
        }
    }
}
