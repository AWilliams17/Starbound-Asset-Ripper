using SharpUtils.MiscUtils;
using SharpUtils.FileUtils;
using SharpUtils.WPFUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Starbound_Asset_Ripper.Classes;
using Starbound_Asset_Ripper.ConfigContainer;
using Starbound_Asset_Ripper.Windows;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Registrar;
using System.Diagnostics;
using System.Net;
using SharpUtils.WebUtils;
using System.Collections.ObjectModel;

namespace Starbound_Asset_Ripper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static Config config = new Config();
        
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            ResizeMode = 0;
            Closing += MainWindow_Closing;

            try
            {
                config.settings.LoadSettings();
            }
            catch (RegLoadException)
            {
                try
                {
                    config.settings.SaveSettings();
                }
                catch (RegSaveException ex)
                {
                    MessageBox.Show($"Failed to save default settings. Error message: {ex.Message}", "Error while saving settings to Registry.");
                }
            }
            // Update the path textboxes
            HandleSteamPath();
            HandleOutputPath();
        }

        public ObservableCollection<Pak> PakListBoxItems
        {
            get { return Pak.PakList; }
        }

        private void OutputPathBtn_Click(object sender, RoutedEventArgs e)
        {
            string outputPath = DialogHelpers.SelectFolderDialog("Select the folder to unload pak contents to.");
            if (outputPath != null)
            {
                config.settings.SetOption("OutputPath", outputPath);
                HandleOutputPath(); // Update the path textbox
            }
        }

        private void SteamPathBtn_Click(object sender, RoutedEventArgs e)
        {
            string steamPath = DialogHelpers.SelectFolderDialog("Select the Steam folder.");
            if (steamPath != null)
            {
                config.settings.SetOption("SteamPath", steamPath);
                HandleSteamPath(); // Update the path textbox
            }
        }

        private void HandleSteamPath()
        {
            string steamPath = config.settings.GetOption<string>("SteamPath");
            if (steamPath != "")
            {
                SteamPathTextBox.Text = steamPath;
                TryLoadPakFiles();
            }
            else
            {
                SteamPathTextBox.Text = "Path to Steam installation...";
                ClearPakDictionary();
            }
        }

        private void HandleOutputPath()
        {
            string outputPath = config.settings.GetOption<string>("OutputPath");
            if (outputPath != "")
            {
                OutputPathTextBox.Text = outputPath;
            }
            else
            {
                OutputPathTextBox.Text = "Path to output folder...";
            }
        }

        private void TryLoadPakFiles()
        {
            string steamPath = config.settings.GetOption<string>("SteamPath");

            if (steamPath != "")
            {
                try
                {
                    Pak.GetPakFiles(steamPath);
                }
                catch (DirectoryNotFoundException ex)
                {
                    MessageBox.Show(ex.Message, "Starbound Workshop Folder not found.");
                    ClearPakDictionary(); // If the path was previously correct, then clear out any detected paks.
                }
            }
        }

        private void ClearPakDictionary()
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

        private async void RedditBtn_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                try
                {
                    string readmeLink = "https://raw.githubusercontent.com/AWilliams17/Starbound-Asset-Ripper/master/README.md";
                    string redditThreadLink = GithubReadmeParser.TryGetLineFromReadme(readmeLink, "Reddit: ", 5);

                    if (redditThreadLink != null)
                    {
                        Process.Start(redditThreadLink);
                    }
                    else MessageBox.Show("Failed to get Reddit thread link from Github Readme.", "Failed to find Reddit thread link");
                }
                catch (WebException ex)
                {
                    MessageBox.Show($"Error occurred while getting Reddit thread link from Github Readme: {ex.Message}", "Error getting Reddit thread link");
                }
            });
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!WindowHelpers.IsWindowOpen(typeof(UpdateWindow)))
            {
                UpdateWindow updateWindow = new UpdateWindow();
                updateWindow.Show();
            }
        }
        
        private void UnpackSelectedBtn_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected values
            // Pass it to the UnpackWindow constructor
            string steamPath = config.settings.GetOption<string>("SteamPath");
            string outputPath = config.settings.GetOption<string>("OutputPath");
            ObservableCollection<Pak> targetPaks = new ObservableCollection<Pak>();

            foreach (Pak pakFile in PakListBox.SelectedItems)
            {
                targetPaks.Add(pakFile);
            }

            BeginUnpackOperation(steamPath, outputPath, targetPaks);
        }

        private void UnpackAllBtn_Click(object sender, RoutedEventArgs e)
        {
            string steamPath = config.settings.GetOption<string>("SteamPath");
            string outputPath = config.settings.GetOption<string>("OutputPath");


            BeginUnpackOperation(steamPath, outputPath, Pak.PakList);
        }

        private void BeginUnpackOperation(string SteamPath, string OutputPath, ObservableCollection<Pak>TargetPaks)
        {
            if (!WindowHelpers.IsWindowOpen(typeof(UnpackWindow)))
            {
                UnpackWindow unpackWindow = new UnpackWindow(SteamPath, OutputPath, TargetPaks);
                unpackWindow.Show();
            }
            else MessageBox.Show("Another unpack operation is currently in progress.");
        }

        private void RefreshPakListBtn_Click(Object sender, RoutedEventArgs e)
        {
            ClearPakDictionary();
            TryLoadPakFiles();
        }

        private void MainWindow_Closing(object sender, EventArgs e)
        {
            config.settings.SaveSettings();
        }
    }
}
