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
        /// <returns>A dictionary with the folder name as a key, and the .pak file path as the value, or null if it didn't find any.</returns>
        public static Dictionary<string, string> TryGetPakFiles(string WorkshopPath)
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

            if (pakFiles.Count != 0)
            {
                return pakFiles;
            }

            else return null;
        }

        /// <summary>
        /// Unpack a .pak file from the given path. Opens the Starbound CLI asset_unpacker.exe and feeds the path to the .pak
        /// file into it, and then returns the result from the asset_unpacker.exe.
        /// </summary>
        /// <param name="SteamPath">The path to the Steam directory.</param>
        /// <param name="PakFilePath">The path to the target .Pak file to unpack.</param>
        /// <param name="OutputPath">The path to spit the contents of the .pak out into.</param>
        public static void UnpackPakFile(string SteamPath, string PakFilePath, string OutputPath)
        {
           
        }

        /// <summary>
        /// Unpacks all .pak files in a listbox into the target directory.
        /// </summary>
        /// <param name="SteamPath">The path to the Steam directory.</param>
        /// <param name="OutputPath">The path to spit the contents of .pak files into. </param>
        /// <param name="PakListBox">The listbox containing the .pak files.</param>
        /// <param name="PakDictionary">The dictionary with details concerning all the .pak files.</param>
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
