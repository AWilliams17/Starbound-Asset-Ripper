using System.Collections.Generic;
using System.Net;
using System.Web.Script.Serialization;

namespace WebUtils
{
    public static class WebRequests
    {
        public static string DownloadString(string Url, int TimeOut)
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

        public static Dictionary<string, object> ParseJson(string JsonUrl, int TimeOut)
        {
            Dictionary<string, object> jsonDictionary = new Dictionary<string, object>();

            if (CanConnect(JsonUrl, TimeOut))
            {
                var json_data = DownloadString(JsonUrl, TimeOut);
                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                var result = jsSerializer.DeserializeObject(json_data);

                jsonDictionary = (Dictionary<string, object>)(result);
            }
            else
            {
                jsonDictionary = null;
            }

            return jsonDictionary;
        }

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
            catch (WebException)
            {
                return false;
            }
        }
    }
}
