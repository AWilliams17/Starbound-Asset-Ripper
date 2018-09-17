using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Starbound_Asset_Ripper
{
    public class AssetUnpacker
    {
        private Process _assetUnpackerProcess;
        private string _steamPath;
        private string _outputPath;
        private string _assetUnpackerPath;

        /// <summary>
        /// Constructor for AssetUnpacker. Throws FileNotFoundException if asset_unpacker.exe was
        /// not found in the Starbound directory in the Steampath.
        /// </summary>
        /// <param name="SteamPath">The path to the Steam directory.</param>
        /// <param name="OutputPath">The path the unpacker will spit the contents of the files out into.</param>
        public AssetUnpacker(string SteamPath, string OutputPath)
        {
            _steamPath = SteamPath;
            _outputPath = OutputPath;
            _assetUnpackerPath = TryGetAssetUnpackerPath(SteamPath);
            _assetUnpackerProcess = new Process
            {
                EnableRaisingEvents = true,
            };
        }

        /// <summary>
        /// Unpack all .pak files in the given dictionary. Opens the Starbound CLI asset_unpacker.exe and feeds the path to the .pak
        /// files into it, and then returns the results from the asset_unpacker.exe.
        /// </summary>
        /// <param name="PakFiles">A dictionary containing key/values of pak file deatils./param>
        /// <returns>Output from the operation.</returns>
        public Task<string> UnpackPakFile(string PakFilePath) // TODO: Should handle ErrorOutput as well.
        {
            Dictionary<string, string> operationResults = new Dictionary<string, string>();
            string assetUnpackerPath = TryGetAssetUnpackerPath(_steamPath);
            string[] assetUnpackerArgs = new string[2] { $"\"{PakFilePath}\"", $"\"{_outputPath}\"" };
            string assetUnpackerOutput = String.Empty;
            TaskCompletionSource<string> taskCompletionSource = new TaskCompletionSource<string>();

            _assetUnpackerProcess.StartInfo = new ProcessStartInfo
            {
                FileName = assetUnpackerPath,
                UseShellExecute = false,
                Arguments = $"{assetUnpackerArgs[0]} {assetUnpackerArgs[1]}",
                RedirectStandardOutput = true,
                CreateNoWindow = true,
            };

            _assetUnpackerProcess.Start();
            _assetUnpackerProcess.BeginOutputReadLine();
            
            _assetUnpackerProcess.OutputDataReceived += (sender, args) =>
            {
                string unpackResult = args.Data;
                taskCompletionSource.TrySetResult(unpackResult);
                _assetUnpackerProcess.CancelOutputRead();
            };

            taskCompletionSource.Task.WaitForCompletionStatus();
            return taskCompletionSource.Task;
        }

        /// <summary>
        /// If an unpack operation is in progress (i.e: asset_unpacker.exe is working on something), this kills it.
        /// </summary>
        public void CancelCurrentOperation()
        {
            try
            {
                _assetUnpackerProcess.Kill();
            }
            catch (InvalidOperationException)
            {
                // This only seems to get thrown if the process was already ended prior to this being
                // called, so it's safe to just catch this and do nothing with it.
            }
        }
        
        /// <summary>
        /// Get the asset_unpacker.exe path from the Steam path. Throws FileNotFoundException if it wasn't found.
        /// </summary>
        /// <param name="SteamPath">Path to the Steam directory.</param>
        /// <returns>The asset unpacker path.</returns>
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
