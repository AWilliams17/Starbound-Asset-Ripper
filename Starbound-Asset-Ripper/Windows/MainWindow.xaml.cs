using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using Starbound_Asset_Ripper.ConfigContainer;
using ApplicationUtils;
using System.Security.Principal;
using System.Threading.Tasks;
using Starbound_Asset_Ripper.Windows;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Controls;
using Registrar;
using System.Diagnostics;

namespace Starbound_Asset_Ripper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        // Misc
        private static Config config = new Config();
        private static ObservableConcurrentDictionary<string, string> pakDictionary = new ObservableConcurrentDictionary<string, string>();

        // Windows
        private static UpdateWindow updateWindow;
        private static UnpackWindow unpackWindow;
        
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            ResizeMode = 0;
            Closing += MainWindow_Closing;

            //if (!(new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator))
            //{
            //    MessageBox.Show("Warning - You are not running as an administrator. It is highly recommended you do so.", "Not Administrator Warning");
            //}

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
                    MessageBox.Show($"Failed to save default settings. Error message: {ex.Message}");
                }
            }
            // Update the path textboxes
            HandleSteamPath();
            HandleOutputPath();
        }

        public ObservableConcurrentDictionary<string, string> PakListBoxItems
        {
            get { return pakDictionary; }
        }

        private void OutputPathBtn_Click(object sender, RoutedEventArgs e)
        {
            string outputPath = FileUtils.DialogHelpers.SelectFolderDialog("Select the folder to unload pak contents to.");
            if (outputPath != null)
            {
                config.settings.SetOption("OutputPath", outputPath);
                HandleOutputPath(); // Update the path textbox
            }
        }

        private void SteamPathBtn_Click(object sender, RoutedEventArgs e)
        {
            string steamPath = FileUtils.DialogHelpers.SelectFolderDialog("Select the Steam folder.");
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
                    string workshopPath = WorkshopPathHelper.TryGetWorkShopPath(steamPath);
                    Dictionary<string, string> pakFiles = PakUtils.GetPakFiles(workshopPath);
                    foreach (KeyValuePair<string, string>kvp in pakFiles)
                    {
                        pakDictionary.Add(kvp.Key, kvp.Value);
                    }
                }
                catch (DirectoryNotFoundException ex)
                {
                    MessageBox.Show(ex.Message);
                    ClearPakDictionary(); // If the path was previously correct, then clear out any detected paks.
                }
            }
        }

        private void ClearPakDictionary()  // Since ObservableConcurrentDictionary doesn't have Clear()?
        {
            foreach (string key in pakDictionary.Keys)
            {
                pakDictionary.Remove(key);
            }
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
            // TOOD: Try-Catch.
            // Also since it can time out make it async.
            string redditThreadLink = WebUtilsRelated.TryGetRedditThread();
            if (redditThreadLink != null)
            {
                Process.Start(redditThreadLink);
            }
            // TODO: Else throw message box.
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen(typeof(UpdateWindow)))
            {
                UpdateWindow updateWindow = new UpdateWindow();
                updateWindow.Show();
            }
        }

        private bool IsWindowOpen(Type WindowType)
        {
            bool windowOpen = false;
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == WindowType)
                {
                    windowOpen = true;
                }
            }
            return windowOpen;
        }
        
        private async void UnpackSelectedBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void UnpackAllBtn_Click(object sender, RoutedEventArgs e)
        {
           
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
