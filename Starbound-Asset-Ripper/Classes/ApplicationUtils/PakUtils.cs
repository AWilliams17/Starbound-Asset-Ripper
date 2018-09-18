using System.Collections.Generic;
using System.IO;

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
    }
}
