using System.IO;
using System.Security.Principal;
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
    }
}
