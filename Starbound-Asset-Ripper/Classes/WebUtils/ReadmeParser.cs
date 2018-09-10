using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUtils
{
    public static class ReadmeParser
    {
        public static string GetLineFromReadme(string ReadmeURL, string LinePrefix, int TimeOut)
        {
            string result = null;

            if (WebRequests.CanConnect(ReadmeURL, TimeOut))
            {
                string readmeText = WebRequests.DownloadString(ReadmeURL, TimeOut);

                using (StringReader sr = new StringReader(readmeText))
                {
                    string currentLine;
                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        if (currentLine.StartsWith(LinePrefix))
                        {
                            result = currentLine.Substring(LinePrefix.Length);
                            break;
                        }
                    }
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
