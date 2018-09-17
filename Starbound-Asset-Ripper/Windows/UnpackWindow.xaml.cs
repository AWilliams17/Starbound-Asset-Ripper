using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Starbound_Asset_Ripper.Windows
{
    /// <summary>
    /// Interaction logic for UnpackWindow.xaml
    /// </summary>
    public partial class UnpackWindow
    {
        private bool canClose = false;
        private string _steamPath;
        private string _outputPath;
        private CancellationTokenSource _cancellationToken;
        private ParallelOptions _parallelOptions;
        private AssetUnpacker _assetUnpacker;
        private Dictionary<string, string[]> _targetPaksDict;
        private Thread _currentThread;

        public UnpackWindow(string SteamPath, string OutputPath, Dictionary<string, string[]>PaksToUnpack)
        {
            InitializeComponent();
            _steamPath = SteamPath;
            _outputPath = OutputPath;
            _targetPaksDict = PaksToUnpack;
            _assetUnpacker = new AssetUnpacker(SteamPath, OutputPath); // TRY CATCH HERE
            _cancellationToken = new CancellationTokenSource();
            _cancellationToken.Token.ThrowIfCancellationRequested();
            _parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount,
                CancellationToken = _cancellationToken.Token,
            };

            Unpack();
        }

        private async void Unpack()
        {
            await Task.Run(() =>
            {
                _currentThread = Thread.CurrentThread;
                int itemsRemaining = _targetPaksDict.Count;
                foreach (KeyValuePair<string, string[]> kvp in _targetPaksDict)
                {
                    string pakPath = kvp.Value[0];
                    string pakFileSize = kvp.Value[1];
                    string result = _assetUnpacker.UnpackPakFile(_steamPath, _outputPath, pakPath);
                    itemsRemaining -= 1;
                    AddResultToListBox(result);
                    SetLabels(kvp.Key, pakFileSize, itemsRemaining);
                }
            });

            _currentThread = null;
            canClose = true;
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
            if (!canClose)
            {
                Task.Factory.StartNew(() =>
                {
                    _cancellationToken.Cancel();
                });
            }
            else Close();
        }
    }
}
