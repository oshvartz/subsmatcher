using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;

namespace SubtitlesMatcher.Infrastructure
{
    public static class WebPageHelper
    {
        public static string RetrieveWebPageContent(string url, Encoding encoding)
        {
            string resposeUrl;
            return RetrieveWebPageContent(url,encoding, out resposeUrl);
        }

        public static string RetrieveWebPageContent(string url,Encoding encoding,out string resposeUrl)
        {

            // *** Establish the request
            HttpWebRequest loHttp =
                 (HttpWebRequest)WebRequest.Create(url);

            string timeoutStr = ConfigurationManager.AppSettings["WebTimeout"];
            int timeout;
            if (!string.IsNullOrEmpty(timeoutStr) && int.TryParse(timeoutStr, out timeout))
            {
                loHttp.Timeout = timeout;
            }
            else
            {

                loHttp.Timeout = 10000;
            }

            // *** Retrieve request info headers
            HttpWebResponse loWebResponse = (HttpWebResponse)loHttp.GetResponse();



            StreamReader loResponseStream =
               new StreamReader(loWebResponse.GetResponseStream(), encoding);

            string lcHtml = loResponseStream.ReadToEnd();

            loWebResponse.Close();
            loResponseStream.Close();

            resposeUrl = loWebResponse.ResponseUri.AbsoluteUri;
            return lcHtml;

        }
    }
}
