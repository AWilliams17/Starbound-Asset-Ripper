using System.Diagnostics;
using WebUtils;

namespace ApplicationUtils
{
    /// <summary>
    /// Application-specific helper class of WebUtils related functions.
    /// </summary>
    public static class WebUtilsRelated
    {
        /// <summary>
        /// Attempt to get the Reddit Thread link from the Github repo's README.md. Throws WebException if anything goes awry/the request times out.
        /// </summary>
        /// <returns>The Reddit Thread link, or null if it wasn't found.</returns>
        public static string TryGetRedditThread()
        {
            string readmeLink = "https://raw.githubusercontent.com/AWilliams17/Starbound-Asset-Ripper/master/README.md";
            string redditLink = GithubReadmeParser.TryGetLineFromReadme(readmeLink, "Reddit: ", 5);
            return redditLink;
        }

        /// <summary>
        /// Just calls Process.Start to open a web browser with the github repo link.
        /// </summary>
        public static void OpenGithubPage()
        {
            Process.Start("https://github.com/AWilliams17/Starbound-Asset-Ripper/");
        }
    }
}
