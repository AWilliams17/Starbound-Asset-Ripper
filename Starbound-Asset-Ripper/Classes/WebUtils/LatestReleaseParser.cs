using System;
using System.Collections.Generic;

namespace WebUtils
{
    public static class LatestReleaseParser
    {
        private static bool RepoFound(string JsonURL, int TimeOut)
        {
            Dictionary<string, object> parsedJson = WebRequests.ParseJson(JsonURL, TimeOut);
            // If the Github API fails to be parsed, it will always contain a message key.
            // From what I have seen, this key isn't present anywhere else in API calls.
            if (parsedJson.ContainsKey("message"))
            {
                if (parsedJson["message"].ToString() == "Not Found")
                {
                    return false;
                }
            }

            return true;
        }

        public static string GetLatestRelease(string GithubUserName, string GithubRepoName, int TimeOut)
        {
            string result = null;
            string fullURL = String.Format("https://api.github.com/repos/{0}/{1}/releases/latest", GithubUserName, GithubRepoName);
            if (WebRequests.CanConnect(fullURL, TimeOut))
            {
                if (RepoFound(fullURL, TimeOut))
                {
                    Dictionary<string, object> parsedJson = WebRequests.ParseJson(fullURL, TimeOut);
                    result = parsedJson["tag_name"].ToString();
                }
                else
                {
                    result = "Failed to find release.";
                }
            }
            else
            {
                result = "Connection Failed.";
            }

            return result;
        }
    }
}
