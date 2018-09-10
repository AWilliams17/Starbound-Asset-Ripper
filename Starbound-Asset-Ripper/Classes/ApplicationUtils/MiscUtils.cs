using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Windows;

namespace ApplicationUtils
{
    public static class MiscUtils
    {
        public static void WarnIfNotAdmin()
        {
            if ((new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator))
            {
                MessageBox.Show("Warning - You are not running as an administrator. It is highly recommended you do so.", "Not Administrator Warning");
            }
        }

        public static string GetWorkShopPath(string SteamPath)
        {
            string workshopPath = string.Format(@"{0}\steamapps\workshop\content\211820\", SteamPath);

            if (Directory.Exists(workshopPath))
            {
                return workshopPath;
            }

            return null;
        }

        public static string SelectFolderDialog(string DialogDescription)
        {
            string selectedPath = null;

            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog()
            {
                Description = DialogDescription,
            };

            System.Windows.Forms.DialogResult folderBrowserResult = folderBrowserDialog.ShowDialog();

            if (folderBrowserResult == System.Windows.Forms.DialogResult.OK)
            {
                selectedPath = folderBrowserDialog.SelectedPath;
            }

            return selectedPath;
        }

        private static string FolderNameFromPath(string PakFilePath)
        {
            string pakName = PakFilePath.Substring(PakFilePath.LastIndexOf('\\'));
            string pakFilePathTrimmed = PakFilePath.Replace(pakName, "");
            string folderName = pakFilePathTrimmed.Substring(pakFilePathTrimmed.LastIndexOf('\\')).Replace("\\", "");

            return folderName;
        }

        public static List<string> UnpackPakFile(string SteamPath, string PakFilePath, string OutputPath)
        {
            List<string> returnValue = new List<string>();
            string operationResult = null;
            string operationError = null;
            string assetUnpackerExePath = $"{SteamPath}\\steamapps\\common\\Starbound\\win32\\asset_unpacker.exe";
            string outputPathOld = OutputPath; // For use in the result since I am going to be modifying this string.
            string folderName = FolderNameFromPath(PakFilePath);


            PakFilePath = PakFilePath.Insert(PakFilePath.Length, "\"").Insert(0, "\"");
            OutputPath = OutputPath.Insert(OutputPath.Length, "\"").Insert(0, "\"");

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = assetUnpackerExePath,
                    UseShellExecute = false,
                    Arguments = $"{PakFilePath} {OutputPath}",
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
                if (operationResult == "")
                {
                    operationResult = operationError;
                    returnValue.Add("Error");
                    returnValue.Add(operationError);
                }
                else
                {
                    operationResult = $"{folderName}: {operationResult.Replace(outputPathOld, "output folder")}";
                    returnValue.Add("Success");
                    returnValue.Add(operationResult);
                }
            }

            return returnValue;
        }
    }
}
