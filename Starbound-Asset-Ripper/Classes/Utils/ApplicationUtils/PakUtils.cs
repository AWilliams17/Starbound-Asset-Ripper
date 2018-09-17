using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;

namespace ApplicationUtils
{
    /// <summary>
    /// A helper class for manipulating Starbound .pak files.
    /// </summary>
    public static class PakUtils
    {
        /// <summary>
        /// Attempts to search for all files in the supplied path ending with a '.pak' file extension.
        /// </summary>
        /// <param name="WorkshopPath">The path to the folder to search for .pak files in.</param>
        /// <returns>A dictionary with the folder name as a key, and the .pak file path as the value.</returns>
        public static Dictionary<string, string[]> GetPakFiles(string WorkshopPath)
        {
            Dictionary<string, string[]> pakFiles = new Dictionary<string, string[]>();
            string[] workshopModFolders = Directory.GetDirectories(WorkshopPath);

            foreach (string folder in workshopModFolders)
            {
                string[] workshopModFolderFiles = Directory.GetFiles(folder);
                foreach (string file in workshopModFolderFiles)
                {
                    if (file.Contains(".pak"))
                    {
                        FileInfo pakFileInfo = new FileInfo(file.TrimEnd('\\'));

                        string dictKey = $".pak in {FileUtils.FolderNameFromPath.GetFolderNameFromFilePath(file)}";
                        string fileSize = FileUtils.FileSizeHelper.GetHumanReadableSize(pakFileInfo.Length);
                        string lastModifiedDate = pakFileInfo.LastWriteTime.ToString("dd/MM/yy HH:mm:ss");

                        string[] dictVal = new string[3] { file, fileSize, lastModifiedDate }; 
                        pakFiles.Add(dictKey, dictVal);
                    }
                }
            }

            return pakFiles;
        }

        /// <summary>
        /// Unpack all .pak files in the given dictionary. Opens the Starbound CLI asset_unpacker.exe and feeds the path to the .pak
        /// the files into it, and then returns the results from the asset_unpacker.exe.
        /// </summary>
        /// <param name="SteamPath">The path to the Steam directory.</param>
        /// <param name="OutputPath">The path to spit the contents of the .pak out into.</param>
        /// <param name="PakFiles">A dictionary containing key/values where the value is the path of a .pak file.</param>
        public static void UnpackPakFile(string SteamPath, string OutputPath, string PakFilePath)
        {
            Dictionary<string, string> operationResults = new Dictionary<string, string>();
            string assetUnpackerPath = TryGetAssetUnpackerPath(SteamPath);
            string[] assetUnpackerArgs = new string[2] { $"\"{PakFilePath}\"", $"\"{OutputPath}\"" };

            if (assetUnpackerPath == null) throw new FileNotFoundException("The Starbound Asset Unpacker CLI was not found.");



        }

        private static string TryGetAssetUnpackerPath(string SteamPath)
        {
            string assetUnpackerPath = $"{SteamPath}\\steamapps\\common\\Starbound\\win32\\asset_unpacker.exe";
            if (File.Exists(assetUnpackerPath))
            {
                return assetUnpackerPath;
            }
            else return null;
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
