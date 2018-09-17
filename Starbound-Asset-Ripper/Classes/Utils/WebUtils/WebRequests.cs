﻿using System;
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
        ///  Custom webclient implementation to allow for custom timeout to be used.
        /// </summary>
        [System.ComponentModel.DesignerCategory("Code")]
        public class WebClientWithTimeout : WebClient
        {
            private readonly int _t;
            /// <summary>
            /// WebclientWithTimeout constructor. Sets it up to use the custom timeout.
            /// </summary>
            /// <param name="timeout">The timeout value in seconds.</param>
            public WebClientWithTimeout(int Timeout)
            {
                _t = Timeout * 1000;
            }
            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest wr = base.GetWebRequest(address);
                wr.Timeout = _t; //in ms
                return wr;
            }
        }

        /// <summary>
        /// Attempts to download a string from a remote URL. Throws WebException if anything goes awry/the request times out.
        /// </summary>
        /// <param name="Url">The URL to download the string from.</param>
        /// <param name="TimeOut">After this specified amount of time in seconds, the request will timeout.</param>
        /// <returns>The downloaded string, or null if it failed to download.</returns>
        public static string TryDownloadString(string Url, int TimeOut)
        {
            string downloadResult = null;
            WebClientWithTimeout webClient = new WebClientWithTimeout(TimeOut);
            webClient.Headers["user-agent"] = "WebUtils Parsing";
            
            downloadResult = webClient.DownloadString(Url);
            
            return downloadResult;
        }

        /// <summary>
        /// Attempts to parse JSON from a remote URL into a dictionary of key/value pairs. Throws WebException if anything goes awry/the request times out.
        /// </summary>
        /// <param name="JsonUrl">The URL to read JSON from.</param>
        /// <param name="TimeOut">After this specified amount of time in seconds, the request will time out.</param>
        /// <returns>A dictionary with the parsed JSON, or null if it failed to parse.</returns>
        public static Dictionary<string, object> TryParseJson(string JsonUrl, int TimeOut)
        {
            Dictionary<string, object> jsonDictionary = new Dictionary<string, object>();

            var json_data = TryDownloadString(JsonUrl, TimeOut);
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            var result = jsSerializer.DeserializeObject(json_data);

            jsonDictionary = (Dictionary<string, object>)(result);

            if (jsonDictionary.Count != 0)
            {
                return jsonDictionary;
            }
            
            else return null;
        }
    }
}
