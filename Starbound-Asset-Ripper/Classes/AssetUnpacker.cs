using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starbound_Asset_Ripper
{
    public class AssetUnpacker
    {
        private Process _assetUnpackerProcess;
        private string _steamPath;
        private string _outputPath;
        private string _assetUnpackerPath;

        public AssetUnpacker(string SteamPath, string OutputPath)
        {
            _steamPath = SteamPath;
            _outputPath = OutputPath;
            _assetUnpackerPath = TryGetAssetUnpackerPath(SteamPath);
            _assetUnpackerProcess = new Process();
        }

        /// <summary>
        /// Unpack all .pak files in the given dictionary. Opens the Starbound CLI asset_unpacker.exe and feeds the path to the .pak
        /// the files into it, and then returns the results from the asset_unpacker.exe.
        /// </summary>
        /// <param name="SteamPath">The path to the Steam directory.</param>
        /// <param name="OutputPath">The path to spit the contents of the .pak out into.</param>
        /// <param name="PakFiles">A dictionary containing key/values where the value is the path of a .pak file.</param>
        /// <returns>Output from the operation.</returns>
        public async Task<string> UnpackPakFile(string SteamPath, string OutputPath, string PakFilePath)
        {
            Dictionary<string, string> operationResults = new Dictionary<string, string>();
            string assetUnpackerPath = TryGetAssetUnpackerPath(SteamPath);
            string[] assetUnpackerArgs = new string[2] { $"\"{PakFilePath}\"", $"\"{OutputPath}\"" };

            _assetUnpackerProcess.StartInfo = new ProcessStartInfo
            {
                FileName = assetUnpackerPath,
                UseShellExecute = false,
                Arguments = $"{assetUnpackerArgs[0]} {assetUnpackerArgs[1]}",
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            
            _assetUnpackerProcess.Start();
            string output = _assetUnpackerProcess.StandardOutput.ReadToEnd();
            _assetUnpackerProcess.WaitForExit();

            return output;
        }

        public void CloseAssetUnpacker()
        {
            if (!_assetUnpackerProcess.HasExited)
            {
                _assetUnpackerProcess.Kill();
            }
        }

        private string TryGetAssetUnpackerPath(string SteamPath)
        {
            string assetUnpackerPath = $"{SteamPath}\\steamapps\\common\\Starbound\\win32\\asset_unpacker.exe";
            if (File.Exists(assetUnpackerPath))
            {
                return assetUnpackerPath;
            }

            throw new FileNotFoundException("The Starbound Asset Unpacker CLI was not found.");
        }
    }
}
