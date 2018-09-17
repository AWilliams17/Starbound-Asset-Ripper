using System.IO;

namespace ApplicationUtils
{
    /// <summary>
    /// Application-specific helper class regarding files.
    /// </summary>
    public static class FileUtils
    {
        /// <summary>
        /// Attempt to get the Starbound workshop path from the Steam directory.
        /// </summary>
        /// <param name="SteamPath">The steam path to look in.</param>
        /// <returns>The workshop path, or null if it was not found.</returns>
        public static string TryGetWorkShopPath(string SteamPath)
        {
            string workshopPath = $"{SteamPath}\\steamapps\\workshop\\content\\211820\\";

            if (Directory.Exists(workshopPath))
            {
                return workshopPath;
            }

            return null;
        }

        /// <summary>
        /// Opens a select folder dialog with the given description.
        /// </summary>
        /// <param name="DialogDescription">The description of the dialog.</param>
        /// <returns>The selected folder, or null if the user did not choose anything.</returns>
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

        private static string RemoveAfter(string str, char c)
        {
            return str.Substring(0, str.LastIndexOf(c));
        }

        private static string RemoveBefore(string str, char c)
        {
            var len = str.LastIndexOf(c);
            return str.Substring(len + 1, str.Length - len - 1);
        }

        /// <summary>
        /// Gets the path to the folder a file is in.
        /// </summary>
        /// <param name="FilePath">The path of the file.</param>
        /// <returns>The folder path.</returns>
        public static string GetFolderNameFromFilePath(string FilePath)
        {
            return RemoveBefore(RemoveAfter(FilePath, '\\'), '\\');
        }
    }
}
