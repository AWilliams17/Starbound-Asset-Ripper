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
        /// <returns></returns>
        public static Dictionary<string, string> GetPakFiles(string WorkshopPath)
        {
            Dictionary<string, string> pakFiles = new Dictionary<string, string>();
            string[] workshopModFolders = Directory.GetDirectories(WorkshopPath);

            foreach (string folder in workshopModFolders)
            {
                string[] workshopModFolderFiles = Directory.GetFiles(folder);
                foreach (string file in workshopModFolderFiles)
                {
                    if (file.Contains(".pak"))
                    {
                        string dictKey = $".pak in {FileUtils.GetFolderNameFromFilePath(file)}";
                        string dictVal = file;
                        pakFiles.Add(dictKey, dictVal);
                    }
                }
            }

            return pakFiles;
        }

        public static void UnpackPakFile(string SteamPath, string PakFilePath, string OutputPath)
        {
           
        }

        public static void UnpackMultiplePaks(string SteamPath, string OutputPath, ListBox PakListBox, Dictionary<string, string> PakDictionary)
        {
            
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
