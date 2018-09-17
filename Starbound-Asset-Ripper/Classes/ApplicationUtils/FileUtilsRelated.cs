using System.IO;

namespace ApplicationUtils
{
    /// <summary>
    /// Application-specific helper class regarding files.
    /// </summary>
    public static class FileUtilsRelated
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
    }
}
