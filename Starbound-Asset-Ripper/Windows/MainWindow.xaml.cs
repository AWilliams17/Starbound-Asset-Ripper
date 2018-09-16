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

namespace Starbound_Asset_Ripper
{
    /*
     *  -PLANNING-
     * 
     * 
    */
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
        static ObservableConcurrentDictionary<string, string> pakDictionary = new ObservableConcurrentDictionary<string, string>();
        static Config config = new Config();
        static string currentStatus = String.Empty;

        // Windows
        private static UpdateWindow updateWindow;

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
        }

        public ObservableConcurrentDictionary<string, string> PakListBoxItems
        {
            get { return pakDictionary; }
        }

        private void PakListBox_ItemSelected(object sender, SelectionChangedEventArgs e)
        {
            if (PakListBox.SelectedItems.Count > 0)
            {
                UnpackSelectedBtn.IsEnabled = true;
            }
            else
            {
                UnpackSelectedBtn.IsEnabled = false;
            }
        }

        private void OutputPathBtn_Click(object sender, RoutedEventArgs e)
        {
            pakDictionary.Add("Test", "Testt");
        }

        private void SteamPathBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HelpBtn_Click(object sender, RoutedEventArgs e)
        {
            WebUtilsRelated.OpenGithubPage();  // TODO: Don't do this
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
            // TRY-CATCH
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

        }

        private async void UnpackAllBtn_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void RefreshPakListBtn_Click(Object sender, RoutedEventArgs e)
        {
            
        }

        private void MainWindow_Closing(object sender, EventArgs e)
        {
            config.settings.SaveSettings();
        }

        private void SetStatusLabel(string LabelText, Brush StatusColor)
        {
            //StatusLabel.Content = LabelText;
            //StatusLabel.Foreground = StatusColor;
        }

        private void UpdateLabels()
        {
            
        }

        private void UpdateButtons()
        {
           
        }

        private void UpdateTextBoxes()
        {
            
        }

        private void UpdatePakList()
        {
            
        }

        private void UpdateAllControls(bool AffectBrowseButtons = false, bool AffectPakButtons = false, bool AffectPakList = false, bool AffectLabels = false)
        {
            
        }
    }
}
