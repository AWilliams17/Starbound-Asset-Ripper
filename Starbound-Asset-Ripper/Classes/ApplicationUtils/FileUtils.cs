using System.IO;

namespace ApplicationUtils
{
    public static class FileUtils
    {
        public static string GetWorkShopPath(string SteamPath)
        {
            string workshopPath = $"{SteamPath}\\steamapps\\workshop\\content\\211820\\";

            if (Directory.Exists(workshopPath))
            {
                return workshopPath;
            }

            throw new DirectoryNotFoundException("The workshop directory does not exist in the provided path.");
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

        private static string RemoveAfter(string str, char c)
        {
            return str.Substring(0, str.LastIndexOf(c));
        }

        private static string RemoveBefore(string str, char c)
        {
            var len = str.LastIndexOf(c);
            return str.Substring(len + 1, str.Length - len - 1);
        }

        public static string GetFolderNameFromFilePath(string FilePath)
        {
            return RemoveBefore(RemoveAfter(FilePath, '\\'), '\\');
        }
    }
}
