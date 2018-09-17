using System;
using System.IO;
using System.Net;

namespace WebUtils
{
    /// <summary>
    /// A helper class for parsing a Github README.MD file.
    /// </summary>
    public static class GithubReadmeParser
    {
        /// <summary>
        /// Attempts to download a string after a given prefix from a README.md. Throws WebException if it timed out.
        /// </summary>
        /// <param name="ReadmeURL">The URL the README.MD is located at.</param>
        /// <param name="LinePrefix">The prefix to search for. This is not included in the result.</param>
        /// <param name="TimeOut">After this specified time, the request will time out.</param>
        /// <returns>The downloaded string if successful, otherwise returns null.</returns>
        public static string TryGetLineFromReadme(string ReadmeURL, string LinePrefix, int TimeOut)
        {
            if (WebRequests.CanConnect(ReadmeURL, TimeOut))
            {
                string readmeText = WebRequests.TryDownloadString(ReadmeURL, TimeOut);

                using (StringReader sr = new StringReader(readmeText))
                {
                    string currentLine;
                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        if (currentLine.StartsWith(LinePrefix))
                        {
                            return currentLine.Substring(LinePrefix.Length);
                        }
                    }
                }
            }
            return null;
        }
    }
}
