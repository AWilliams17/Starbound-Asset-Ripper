using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;

namespace ApplicationUtils
{
    public static class PakUtils
    {
        private static string[] PakFileResult(string Status, string ResultMessage)
        {
            return new string[] { Status, ResultMessage };
        }

        public static string UnpackPakFile(string SteamPath, string PakFilePath, string OutputPath)
        {
            string operationResult = null;
            string operationError = null;
            string assetUnpackerExePath = $"{SteamPath}\\steamapps\\common\\Starbound\\win32\\asset_unpacker.exe";
            string folderName = FileUtils.GetFolderNameFromFilePath(PakFilePath);

            if (!File.Exists(assetUnpackerExePath))
            {
                throw new FileNotFoundException("'asset_unpacker.exe' was not found in the Starbound folder.");
            }

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = assetUnpackerExePath,
                    UseShellExecute = false,
                    Arguments = $"\"{PakFilePath}\"" + $" \"{OutputPath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                }
            };
            proc.Start();
            StreamReader assetUnpackerOutput = proc.StandardOutput;
            StreamReader assetUnpackerError = proc.StandardError;
            proc.WaitForExit();
            if (proc.HasExited)
            {
                operationResult = assetUnpackerOutput.ReadToEnd();
                operationError = assetUnpackerError.ReadToEnd();
                if (operationResult == "" && operationError != "")
                {
                    throw new UnpackPakException(operationResult);
                }

                operationResult = $"{folderName}: {operationResult.Replace(OutputPath, "output folder")}";
            }
            
            return operationResult;
        }

        public static string[][] UnpackMultiplePaks(string SteamPath, string OutputPath, ListBox PakListBox, Dictionary<string, string> PakDictionary)
        {
            int currItemIndex = 0;
            int itemsToProcess = PakListBox.Items.Count;
            string[] failedItems = new string[itemsToProcess];
            string[] processedItems = new string[itemsToProcess];
            
            foreach (string currentPakFile in PakListBox.Items)
            {
                int itemsRemaining = itemsToProcess - currItemIndex;
                string currentPakPath = PakDictionary[currentPakFile];
                string unpackFileResult = null;

                try
                {
                    unpackFileResult = UnpackPakFile(SteamPath, currentPakPath, OutputPath);
                    processedItems[currItemIndex] = currentPakFile;
                }
                catch (UnpackPakException ex)
                {
                    failedItems[currItemIndex] = $"Failed to process {currentPakFile}: {ex.Message}{Environment.NewLine}";
                }
                currItemIndex += 1;
            }

            return new string[][] { processedItems, failedItems };

        }
    }
    
    public class UnpackPakException : Exception
    {
        public UnpackPakException() { }
        public UnpackPakException(string message) : base(message) { }
        public UnpackPakException(string message, Exception inner) : base(message, inner) { }
        protected UnpackPakException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
