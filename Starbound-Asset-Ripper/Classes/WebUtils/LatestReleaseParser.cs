using System;
using System.Collections.Generic;
using System.Net;

namespace WebUtils
{
    /// <summary>
    /// A helper class for getting the latest release from a Github repo.
    /// </summary>
    public static class LatestReleaseParser
    {
        /// <summary>
        /// Attempts to find the repo at the given URL. Throws WebException if it timed out, or if it failed to parse the JSON.
        /// </summary>
        /// <param name="JsonURL">The URL to find the repo at.</param>
        /// <param name="TimeOut">After this specified time, the request will time out.</param>
        /// <returns>True if it found the repo, false otherwise.</returns>
        private static bool RepoFound(string JsonURL, int TimeOut)
        {
            Dictionary<string, object> parsedJson = WebRequests.TryParseJson(JsonURL, TimeOut);
            // If the Github API fails to be parsed, it will always contain a message key.
            // From what I have seen, this key isn't present anywhere else in API calls.
            if (parsedJson != null)
            {
                if (parsedJson.ContainsKey("message"))
                {
                    if (parsedJson["message"].ToString() == "Not Found")
                    {
                        return false;
                    }
                }
                return true;
            }
            throw new WebException($"Failed to parse JSON at {JsonURL}");
        }

        /// <summary>
        /// Attempt to get the latest release from the repo. Throws WebException if it timed out, or if it failed to parse the JSON.
        /// </summary>
        /// <param name="GithubUserName">The username which the repo can be found under.</param>
        /// <param name="GithubRepoName">The name of the repo to get the latest release from.</param>
        /// <param name="TimeOut">After this specified amount of time, the check times out.</param>
        /// <returns>The latest release tag if successful, otherwise null.</returns>
        public static string TryGetLatestRelease(string GithubUserName, string GithubRepoName, int TimeOut)
        {
            string fullURL = String.Format("https://api.github.com/repos/{0}/{1}/releases/latest", GithubUserName, GithubRepoName);
            if (WebRequests.CanConnect(fullURL, TimeOut))
            {
                if (RepoFound(fullURL, TimeOut))
                {
                    Dictionary<string, object> parsedJson = WebRequests.TryParseJson(fullURL, TimeOut);
                    if (parsedJson != null)
                    {
                        return parsedJson["tag_name"].ToString();
                    }
                }
            }
            return null;
        }
    }
}
