﻿using SharpUtils.FileUtils;
using Starbound_Asset_Ripper.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        private ObservableCollection<Pak> _targetPaks;

        public UnpackWindow(string SteamPath, string OutputPath, ObservableCollection<Pak>PaksToUnpack)
        {
            InitializeComponent();
            _steamPath = SteamPath;
            _outputPath = OutputPath;
            _targetPaks = PaksToUnpack;
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
                int itemsRemaining = _targetPaks.Count;
                
                foreach (Pak pak in _targetPaks)
                {
                    string pakFileSize = FileSizeHelper.GetHumanReadableSize(pak.PakFileSize);
                    string result = null;
                    SetLabels(pak.PakFolderName, pakFileSize, itemsRemaining);
                    result = await _assetUnpacker.UnpackPakFile(pak.PakFilePath);
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

        private void UnpackWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _assetUnpacker.CancelCurrentOperation();
            _taskRunning = false;
        }
    }
}
