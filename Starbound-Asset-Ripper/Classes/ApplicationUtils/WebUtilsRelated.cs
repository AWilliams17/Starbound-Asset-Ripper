using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Windows;
using WebUtils;

namespace ApplicationUtils
{
    public static class WebUtilsRelated
    {
        public static bool GetUpdateAvailable()
        {
            string current_version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string latestRelease = LatestReleaseParser.TryGetLatestRelease("AWilliams17", "Starbound-Asset-Ripper", 5);
            if (latestRelease != null)
            {
                if (current_version != latestRelease)
                {
                    return true;
                }
                return false;
            }
            throw new WebException("Failed to find latest release.");
        }

        public static string TryGetRedditThread()
        {
            string readmeLink = "https://raw.githubusercontent.com/AWilliams17/Starbound-Asset-Ripper/master/README.md";
            string redditLink = GithubReadmeParser.TryGetLineFromReadme(readmeLink, "Reddit: ", 5);
            return redditLink;
        }

        public static void OpenGithubPage()
        {
            Process.Start("https://github.com/AWilliams17/Starbound-Asset-Ripper/");
        }
    }
}
