using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ApplicationUtils
{
    public static class PakUtils
    {
        private static string[] PakFileResult(string Status, string ResultMessage)
        {
            return new string[] { Status, ResultMessage };
        }

        public static string[] UnpackPakFile(string SteamPath, string PakFilePath, string OutputPath)
        {
            string operationResult = null;
            string operationError = null;
            string returnStatus = null;
            string assetUnpackerExePath = $"{SteamPath}\\steamapps\\common\\Starbound\\win32\\asset_unpacker.exe";
            string folderName = FileUtils.GetFolderNameFromFilePath(PakFilePath);

            if (!File.Exists(assetUnpackerExePath))
            {
                return PakFileResult("Error_EXE", "Asset_Unpacker.exe not found in Starbound\\win32\\");
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
                    operationResult = operationError;
                    returnStatus = "Error";

                }
                else if (operationResult != "")
                {
                    operationResult = $"{folderName}: {operationResult.Replace(OutputPath, "output folder")}";
                    returnStatus = "Success";
                }
                else
                {
                    operationResult = "Failed to parse: Unknown error occurred.";
                    returnStatus = "Error";
                }
            }
            
            return PakFileResult(returnStatus, operationResult); ;
        }

        public static string[][] UnpackMultiplePaks(string SteamPath, string OutputPath, ListBox PakListBox, Dictionary<string, string> PakDictionary)
        {
            int currItemIndex = 0;
            int itemsToProcess = PakListBox.Items.Count;
            string[] failedItems = new string[itemsToProcess];
            string[] processedItems = new string[itemsToProcess];
            string[] exeMissingErrorMessage = new string[] { "EXE_Missing", "" };
            
            foreach (string currentPakFile in PakListBox.Items)
            {
                int itemsRemaining = itemsToProcess - currItemIndex;
                string currentPakPath = PakDictionary[currentPakFile];

                string[] unpackFileResult = UnpackPakFile(SteamPath, currentPakPath, OutputPath);

                if (unpackFileResult[0] == "Success")
                {
                    processedItems[currItemIndex] = currentPakFile;
                }
                else if (unpackFileResult[1] == "Error")
                {
                    failedItems[currItemIndex] = $"Failed to process {currentPakFile}: {unpackFileResult[1]}{Environment.NewLine}";
                }
                else
                {
                    exeMissingErrorMessage[1] = unpackFileResult[1];
                    break;
                }
                currItemIndex += 1;
            }
            return new string[][] { processedItems, failedItems, exeMissingErrorMessage };

        }
    }
}
