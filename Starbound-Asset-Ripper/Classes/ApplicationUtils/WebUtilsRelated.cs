using System.Diagnostics;
using System.Reflection;
using System.Windows;
using WebUtils;

namespace ApplicationUtils
{
    public static class WebUtilsRelated
    {
        public static bool UpdateAvailable()
        {
            string current_version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string latestRelase = LatestReleaseParser.GetLatestRelease("AWilliams17", "Starbound-Asset-Ripper", 5);
            if (current_version != latestRelase)
            {
                return true;
            }
            return false;
        }

        public static void OpenRedditThread()
        {
            string readmeLink = "https://raw.githubusercontent.com/AWilliams17/Starbound-Asset-Ripper/master/README.md";
            string redditLink = ReadmeParser.GetLineFromReadme(readmeLink, "Reddit: ", 5);
            Process.Start(redditLink);
        }

        public static void OpenGithubPage()
        {
            Process.Start("https://github.com/AWilliams17/Starbound-Asset-Ripper/");
        }
    }
}
