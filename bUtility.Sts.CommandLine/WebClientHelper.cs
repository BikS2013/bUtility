using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts.CommandLine
{
    static class WebClientHelper
    {
        const string UserAgent = "WindowsNT/7";

        private static WebResponse ExecuteHttpVerb(string url, string verbData, CookieContainer cookieContainer = null, string contentType = "application/x-www-form-urlencoded", string verb = "POST")
        {
            try
            {

                WebRequest request = WebRequest.Create(url);

                HttpWebRequest wr = request as HttpWebRequest;
                if (cookieContainer != null)
                    wr.CookieContainer = cookieContainer;
                wr.AllowAutoRedirect = false;
                request.Method = verb;
                wr.Proxy = null;
                wr.UserAgent = UserAgent;
                if (!string.IsNullOrEmpty(verbData))
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(verbData);
                    request.ContentType = contentType;
                    request.ContentLength = byteArray.Length;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                }
                WebResponse response = request.GetResponse();

                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static WebResponse Post(string url, string postData, CookieContainer cookieContainer = null, string contentType = "application/x-www-form-urlencoded", string xcrfToken = null)
        {
            return ExecuteHttpVerb(url, postData, cookieContainer, contentType, "POST");
        }


        public static WebResponse Get(string url, string getData, CookieContainer cookieContainer = null, string contentType = "application/x-www-form-urlencoded", string xcrfToken = null)
        {
            return ExecuteHttpVerb(url, getData, cookieContainer, contentType, "GET");
        }


    }
}
