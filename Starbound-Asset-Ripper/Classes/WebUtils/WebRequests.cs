using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Script.Serialization;

namespace WebUtils
{
    /// <summary>
    /// A helper class for making simple web requests.
    /// </summary>
    public static class WebRequests
    {
        /// <summary>
        /// Attempts to download a string from a remote URL. Throws WebException if it timed out.
        /// </summary>
        /// <param name="Url">The URL to download the string from.</param>
        /// <param name="TimeOut">After this specified time, the request will time out.</param>
        /// <returns>The downloaded string, or null if it failed to download.</returns>
        public static string TryDownloadString(string Url, int TimeOut)
        {
            string downloadResult = null;
            WebClient webClient = new WebClient();
            webClient.Headers["user-agent"] = "WebUtils Parsing";

            if (CanConnect(Url, TimeOut))
            {
                downloadResult = webClient.DownloadString(Url);
            }
            
            return downloadResult;
        }

        /// <summary>
        /// Attempts to parse JSON from the remote url into a dictionary of key/value pairs. Throws WebException if it timed out.
        /// </summary>
        /// <param name="JsonUrl">The URL to read JSON from.</param>
        /// <param name="TimeOut">After this specified time, the request will time out.</param>
        /// <returns>A dictionary with the parsed JSON, or null if it failed to parse.</returns>
        public static Dictionary<string, object> TryParseJson(string JsonUrl, int TimeOut)
        {
            Dictionary<string, object> jsonDictionary = new Dictionary<string, object>();

            if (CanConnect(JsonUrl, TimeOut))
            {
                var json_data = TryDownloadString(JsonUrl, TimeOut);
                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                var result = jsSerializer.DeserializeObject(json_data);

                jsonDictionary = (Dictionary<string, object>)(result);
            }
            else return null;

            return jsonDictionary;
        }

        /// <summary>
        /// Checks to see if a connection can be made to the given url. Throws WebException if it timed out.
        /// </summary>
        /// <param name="Url">The URL to check.</param>
        /// <param name="TimeOut">After this specified time, the request will time out.</param>
        /// <returns>True if a successful connection is made, false otherwise.</returns>
        public static bool CanConnect(string Url, int TimeOut)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Timeout = TimeOut * 1000; // Miliseconds
            request.Method = "HEAD";
            request.UserAgent = "WebUtils Parsing";
            request.AllowAutoRedirect = true;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    int statusCode = (int)response.StatusCode;
                    return (statusCode >= 200 && statusCode <= 299);
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.Timeout)
                {
                    throw;
                }
                else return false;
            }
        }
    }
}
