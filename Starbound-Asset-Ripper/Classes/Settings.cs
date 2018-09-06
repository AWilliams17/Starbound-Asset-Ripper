using Microsoft.Win32;

namespace Starbound_Asset_Ripper
{
    public class Settings
    {
        private string _steamDirectory = "";
        private string _outputDirectory = "";

        public string SteamDirectory
        {
            get => _steamDirectory;
            set => _steamDirectory = value;
        }

        public string OutputDirectory
        {
            get => _outputDirectory;
            set => _outputDirectory = value;
        }

        public void SaveSettings()
        {
            Registry.SetValue("HKEY_CURRENT_USER\\Software\\StarboundAssetRipper", "SteamDirectory", SteamDirectory, RegistryValueKind.String);
            Registry.SetValue("HKEY_CURRENT_USER\\Software\\StarboundAssetRipper", "OutputDirectory", OutputDirectory, RegistryValueKind.String);
        }

        public void LoadSettings() // This is bad, but it works.
        {
            RegistryKey StarboundAssetRipperRegistry = Registry.CurrentUser.OpenSubKey("Software\\StarboundAssetRipper", false);
            if (StarboundAssetRipperRegistry == null)
            {
                SaveSettings();
            }
            else
            {
                SteamDirectory = StarboundAssetRipperRegistry.GetValue("SteamDirectory").ToString();
                OutputDirectory = StarboundAssetRipperRegistry.GetValue("OutputDirectory").ToString();
            }
        }
    }

    class Validators
    {

    }
}