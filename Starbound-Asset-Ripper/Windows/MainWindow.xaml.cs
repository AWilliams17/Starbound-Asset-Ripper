﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using Starbound_Asset_Ripper.ConfigContainer;
using ApplicationUtils;
using System.Security.Principal;
using System.ComponentModel;
using System.Threading.Tasks;
using Starbound_Asset_Ripper.Windows;

// TODO: Need to add threading around the web utils function calls
namespace Starbound_Asset_Ripper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        // Paths
        private bool steamPathSet = false;
        private bool outputPathSet = false;
        private bool workshopPathSet = false;

        // Misc
        private Dictionary<string, string> pakDictionary = new Dictionary<string, string>();
        public static Config config = new Config();

        // Windows
        private static UpdateWindow updateWindow;

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
                MessageBox.Show(loadSettingsResult);
                config.settings.SaveSettings();
            }
            steamPathSet = (config.settings.GetOption<string>("SteamPath") != "");
            workshopPathSet = (config.settings.GetOption<string>("WorkshopPath") != "");
            outputPathSet = (config.settings.GetOption<string>("OutputPath") != "");

            UpdateAllControls(true, true);
        }

        private void OutputPathBtn_Click(object sender, RoutedEventArgs e)
        {
            string outputPath = FileUtils.SelectFolderDialog("");
            if (outputPath != null)
            {
                config.settings.SetOption("OutputPath", outputPath);
                config.settings.SaveSettings();
                outputPathSet = true;
                UpdateAllControls(true, true);
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
                UpdateAllControls(true, true);
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
            bool windowOpen = false;

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(UpdateWindow))
                {
                    windowOpen = true;
                }
            }

            if (!windowOpen)
            {
                updateWindow = new UpdateWindow();
                updateWindow.Show();
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

                // For errors
                string unpackError = null;
                string exeMissingError = null;

                SetStatusLabel($"Unpacking {selectedValue}... This might take a moment.", LabelColors.Good);

                UpdateAllControls(AffectButtons: true, AffectLabels: false);

                string unpackFileResult = await Task.Run(() =>
                {
                    try
                    {
                        return PakUtils.UnpackPakFile(steamPath, selectedPakPath, outputPath);
                    }
                    catch (UnpackPakException ex)
                    {
                        unpackError = ex.Message;
                    }
                    catch (FileNotFoundException ex)
                    {
                        exeMissingError = ex.Message;
                    }
                    return null;
                });
                
                if (unpackError != null)
                {
                    MessageBox.Show($"Error occurred while unpacking {selectedValue}:{Environment.NewLine}{unpackFileResult[1]}");
                    SetStatusLabel($"Failed to unpack {selectedValue}", LabelColors.Bad);
                }
                else if (exeMissingError != null)
                {
                    MessageBox.Show($"Failed to unpack: asset_unpacker.exe was not found in the Starbound folder.");
                    SetStatusLabel($"Failed to unpack: asset_unpacker.exe is missing.", LabelColors.Bad);
                }
                else
                {
                    SetStatusLabel(unpackFileResult, LabelColors.Good);
                }
                
                UpdateAllControls(AffectButtons: true, AffectLabels: false);
            }
        }

        private async void UnpackAllBtn_Click(object sender, RoutedEventArgs e)
        {
            SetStatusLabel($"Unpacking all pak files... This might take a moment.", LabelColors.Good);

            UpdateAllControls(AffectButtons: true, AffectLabels: false);

            DateTime operationStartTime = DateTime.Now;
            string steamPath = config.settings.GetOption<string>("SteamPath");
            string outputPath = config.settings.GetOption<string>("OutputPath");
            string exeMissingError = null;
            int itemsToProcess = PakListBox.Items.Count;
            
            string[][] unpackAllResult = await Task.Run(() =>
            {
                try
                {
                    return PakUtils.UnpackMultiplePaks(steamPath, outputPath, PakListBox, pakDictionary);
                }
                catch (FileNotFoundException ex)
                {
                    exeMissingError = ex.Message;
                }
                return null;
            });
            
            string[] processedItems = unpackAllResult[0];
            string[] failedItems = unpackAllResult[1];
            if (exeMissingError != null)
            {
                MessageBox.Show(exeMissingError);
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

            UpdateAllControls(AffectButtons: true, AffectLabels: false);
        }

        private void RefreshPakListBtn_Click(Object sender, RoutedEventArgs e)
        {
            UpdateAllControls(true, true, false);
            UpdatePakList();
            UpdateAllControls(true, true, false);
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
                    SetStatusLabel("Everything looks good.", LabelColors.Good);
                }
            }
            else
            {
                SetStatusLabel("Waiting for paths to be set...", LabelColors.Bad);
            }
        }

        private void UpdateButtons()
        {
            SteamPathBtn.IsEnabled = !SteamPathBtn.IsEnabled;
            OutputPathBtn.IsEnabled = !OutputPathBtn.IsEnabled;
            UnpackAllBtn.IsEnabled = !UnpackAllBtn.IsEnabled;
            UnpackSelectedBtn.IsEnabled = !UnpackSelectedBtn.IsEnabled;
            RefreshPakListBtn.IsEnabled = !RefreshPakListBtn.IsEnabled;
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
            PakListBox.Items.Clear();
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
                        PakListBox.Items.Add(pakDictKey);
                    }
                }
            }
        }

        private void UpdateAllControls(bool AffectButtons = false, bool AffectPakList = false, bool AffectLabels = true)
        {
            UpdateTextBoxes();
            if (steamPathSet && workshopPathSet && outputPathSet)
            {
                if (AffectPakList)
                {
                    UpdatePakList();
                }
                if (AffectButtons)
                {
                    UpdateButtons();
                }
                if (AffectLabels)
                {
                    UpdateLabels();
                }
            }
        }
    }
}
