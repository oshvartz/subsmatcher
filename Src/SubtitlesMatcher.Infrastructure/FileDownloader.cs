using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Configuration;

namespace SubtitlesMatcher.Infrastructure
{
    public static class FileDownloader
    {
        public static void Download(string url, string targetFilePath)
        {
            InnerWebClient client = new InnerWebClient();

            client.Timeout = int.MaxValue;
            
            client.DownloadFile(url, targetFilePath);
        }


        internal class InnerWebClient : WebClient
        {
            private System.Net.CookieContainer cookieContainer;
            private string userAgent;
            private int timeout;

            public System.Net.CookieContainer CookieContainer
            {
                get { return cookieContainer; }
                set { cookieContainer = value; }
            }

            public string UserAgent
            {
                get { return userAgent; }
                set { userAgent = value; }
            }

            public int Timeout
            {
                get { return timeout; }
                set { timeout = value; }
            }

            public InnerWebClient()
            {
                timeout = -1;
                
                cookieContainer = new CookieContainer();
            }

            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest request = base.GetWebRequest(address);

                if (request.GetType() == typeof(HttpWebRequest))
                {
                    ((HttpWebRequest)request).CookieContainer = cookieContainer;
                    ((HttpWebRequest)request).UserAgent = userAgent;
                    ((HttpWebRequest)request).Timeout = timeout;
                }

                return request;
            }
        }  
    }
}
