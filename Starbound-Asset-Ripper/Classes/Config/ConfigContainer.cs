using Starbound_Asset_Ripper.ConfigValidators;


namespace Starbound_Asset_Ripper.ConfigContainer
{
    public class Config
    {
        public Registrar.RegSettings settings = new Registrar.RegSettings(Registrar.RegBaseKeys.HKEY_CURRENT_USER, "Software/StarboundAssetRipper");
        private Validators validators = new Validators();

        private void RegisterSettings()
        {
            Registrar.RegOption steamPath = new Registrar.RegOption("SteamPath", validators.DirectoryValidator, "", typeof(string));
            Registrar.RegOption outputPath= new Registrar.RegOption("OutputPath", validators.DirectoryValidator, "", typeof(string));

            settings.RegisterSetting("SteamPath", steamPath);
            settings.RegisterSetting("OutputPath", outputPath);
        }

        public Config()
        {
            RegisterSettings();
        }
    }
}
