using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace Starbound_Asset_Ripper.Classes
{
    /// <summary>
    /// Represents a .pak file
    /// </summary>
    public class Pak
    {
        public static ObservableCollection<Pak> PakList = new ObservableCollection<Pak>();
        public DateTime PakLastModified { get; private set; }
        public long PakFileSize { get; private set; }
        public string PakFilePath { get; private set; }
        public string PakFolderName { get; private set; }

        /// <summary>
        /// Constructor for the Pak class.
        /// </summary>
        /// <param name="PakFilePath">The path to the .pak file.</param>
        /// <param name="PakFileSize">The size of the .pak file.</param>
        /// <param name="LastModifiedDate">The last modified date of the .pak file.</param>
        public Pak(string PakPath, long PakSize, DateTime PakLastModifiedDate)
        {
            PakFilePath = PakFilePath;
            PakFileSize = PakFileSize;
            PakLastModified = PakLastModifiedDate;
            PakFolderName = GetPakFolderName(PakPath);
        }

        /// <summary>
        /// Gets the name of the folder a .pak file is located in.
        /// </summary>
        /// <returns>The name of the folder the .pak file is in.</returns>
        private string GetPakFolderName(string FilePath)
        {
            string containingFolder = Path.GetDirectoryName(FilePath);
            string folderName = containingFolder.Substring(containingFolder.LastIndexOf('\\')).Trim('\\');
            return folderName;
        }

        /// <summary>
        /// Searches through the Steam Workshop folder for .pak files and then creates instances
        /// of the Pak class and throws them in the PakList.
        /// </summary>
        /// <param name="SteamPath">The path to the Steam folder.</param>
        public static void GetPakFiles(string SteamPath)
        {
            string[] workshopModFolders = Directory.GetDirectories(TryGetWorkShopPath(SteamPath));

            foreach (string folder in workshopModFolders)
            {
                string[] workshopModFolderFiles = Directory.GetFiles(folder);
                foreach (string file in workshopModFolderFiles)
                {
                    if (file.Contains(".pak"))
                    {
                        FileInfo pakFileInfo = new FileInfo(file.TrimEnd('\\'));
                        DateTime pakLastModifiedDate = pakFileInfo.LastWriteTime;
                        long pakFileSize = pakFileInfo.Length;
                        string pakFilePath = file;
                        Pak pakFile = new Pak(pakFilePath, pakFileSize, pakLastModifiedDate);
                        PakList.Add(pakFile);
                    }
                }
            }
        }

        /// <summary>
        /// Attempt to get the Starbound workshop path from the Steam directory.
        /// </summary>
        /// <param name="SteamPath">The steam path to look in.</param>
        /// <returns>The workshop path, or null if it was not found.</returns>
        private static string TryGetWorkShopPath(string SteamPath)
        {
            string workshopPath = $"{SteamPath}\\steamapps\\workshop\\content\\211820\\";

            if (Directory.Exists(workshopPath))
            {
                return workshopPath;
            }

            throw new DirectoryNotFoundException("The Starbound workshop folder was not found in the Steam path. Do you have any mods installed?");
        }
    }
}
