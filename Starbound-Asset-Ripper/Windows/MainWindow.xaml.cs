﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using Starbound_Asset_Ripper.ConfigContainer;
using ApplicationUtils;

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

            UpdateAllControls();
        }

        private void OutputPathBtn_Click(object sender, RoutedEventArgs e)
        {
            string outputPath = MiscUtils.SelectFolderDialog("");
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

        private void UnpackSelectedBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UnpackAllBtn_Click(object sender, RoutedEventArgs e)
        {

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
