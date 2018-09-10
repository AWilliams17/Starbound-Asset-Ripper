﻿using System.Collections.Generic;
using System.Net;
using System.Web.Script.Serialization;

namespace Web.Utils
{
    public static class WebUtils
    {
        public static string DownloadString(string Url, int TimeOut)
        {
            string downloadResult = null;
            WebClient webClient = new WebClient();

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
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (WebException)
            {
                return false;
            }
        }
    }
}