using Starbound_Asset_Ripper.ConfigValidators;


namespace Starbound_Asset_Ripper.ConfigContainer
{
    public class Config
    {
        public Registrar.RegSettings settings = new Registrar.RegSettings(Registrar.BaseKeys.HKEY_CURRENT_USER, "Software/StarboundAssetRipper");
        private Validators validators = new Validators();

        private void RegisterSettings()
        {
            Registrar.RegOption steamDirectory = new Registrar.RegOption("SteamDirectory", validators.DirectoryValidator, "", typeof(string));
            Registrar.RegOption outputDirectory = new Registrar.RegOption("OutputDirectory", validators.DirectoryValidator, "", typeof(string));
            Registrar.RegOption workshopDirectory = new Registrar.RegOption("WorkshopDirectory", validators.DirectoryValidator, "", typeof(string));

            settings.RegisterSetting("SteamDirectory", steamDirectory);
            settings.RegisterSetting("OutputDirectory", outputDirectory);
            settings.RegisterSetting("WorkshopDirectory", workshopDirectory);
        }

        public Config()
        {
            RegisterSettings();
        }
        
    }
}
