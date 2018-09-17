using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Starbound_Asset_Ripper.Windows
{
    /// <summary>
    /// Interaction logic for UnpackWindow.xaml
    /// </summary>
    public partial class UnpackWindow
    {
        private bool _canClose = false;
        private bool _taskRunning = true;
        private string _steamPath;
        private string _outputPath;
        private AssetUnpacker _assetUnpacker;
        private Dictionary<string, string[]> _targetPaksDict;

        public UnpackWindow(string SteamPath, string OutputPath, Dictionary<string, string[]>PaksToUnpack)
        {
            InitializeComponent();
            _steamPath = SteamPath;
            _outputPath = OutputPath;
            _targetPaksDict = PaksToUnpack;
            try
            {
                _assetUnpacker = new AssetUnpacker(SteamPath, OutputPath);
                Unpack();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "asset_unpacker.exe was not found.");
                Close();
            }
        }
        
        private async void Unpack()
        {
            await Task.Run(async () =>
            {
                int itemsRemaining = _targetPaksDict.Count;
                
                foreach (KeyValuePair<string, string[]> kvp in _targetPaksDict)
                {
                    string pakPath = kvp.Value[0];
                    string pakFileSize = kvp.Value[1];
                    string result = null;
                    SetLabels(kvp.Key, pakFileSize, itemsRemaining);
                    result = await _assetUnpacker.UnpackPakFile(pakPath);
                    if (!_taskRunning) break;
                    itemsRemaining -= 1;
                    AddResultToListBox(result);
                }
            });
            _canClose = true;
            CancelBtn.Content = "Close";
            SetLabels("No operation in progress.", "Not unpacking anything.", 0);
        }

        private void SetLabels(string PakKey, string FileSize, int ItemsRemaining)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                PakKeyLabel.Content = PakKey;
                PakFileSizeLabel.Content = FileSize;
                ItemsRemainingLabel.Content = ItemsRemaining.ToString();
            }));
        }

        private void AddResultToListBox(string Result)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                ResultsListBox.Items.Add(Result);
            }));
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!_canClose)
            {
                _assetUnpacker.CancelCurrentOperation();
                _taskRunning = false;
            }
            else Close();
        }
    }
}
