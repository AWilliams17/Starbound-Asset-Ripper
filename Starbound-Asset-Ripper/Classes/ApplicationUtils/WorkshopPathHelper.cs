using System.IO;

namespace ApplicationUtils
{
    /// <summary>
    /// Application-specific helper class which contains a function for getting the workshop path from the Steam directory.
    /// Probably going to just move this somewhere else and delete this file.
    /// </summary>
    public static class WorkshopPathHelper
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

            throw new DirectoryNotFoundException("The Starbound workshop folder was not found in the Steam path. Do you have any mods installed?");
        }
    }
}
