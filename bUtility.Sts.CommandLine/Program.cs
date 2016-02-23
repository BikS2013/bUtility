using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace bUtility.Sts.CommandLine
{
    class Program
    {
        private static readonly string username = "ttt";
        private static readonly string password = "ppp";
        static void Main(string[] args)
        {
            var postData = $"userName={username}&password={username}&Id=test";
            var loginUrl = ConfigurationManager.AppSettings["loginUrl"];
            var testUrl = ConfigurationManager.AppSettings["testUrl"];
            var _loginCookies = new CookieContainer();

            string stepTwoPostData = "";
            using (var loginResponse = WebClientHelper.Post(loginUrl, postData, _loginCookies))
            {

                if (loginResponse != null)
                {
                    var resp = loginResponse as HttpWebResponse;
                    if (resp != null && resp.StatusCode == HttpStatusCode.OK)
                    {
                        using (var reader = new StreamReader(loginResponse.GetResponseStream()))
                        {
                            HtmlDocument doc = new HtmlDocument();
                            doc.Load(reader);

                            HtmlNode formNode = doc.DocumentNode.Descendants("form")
                                .First(el =>
                                    (el.Attributes["name"] != null &&
                                    el.Attributes["name"].Value == "hiddenform"));

                            if (formNode != null)
                            {
                                var hiddenPostInputs = doc.DocumentNode.Descendants("input").Where(el =>
                                    el.Attributes["type"] != null
                                    && el.Attributes["type"].Value == "hidden"
                                    && el.Attributes["value"] != null
                                );
                                foreach (var input in hiddenPostInputs)
                                {
                                    if (!string.IsNullOrEmpty(stepTwoPostData))
                                        stepTwoPostData += "&";
                                    stepTwoPostData += $"{input.Attributes["name"].Value}={HttpUtility.UrlEncode(WebUtility.HtmlDecode(input.Attributes["value"].Value))}";
                                }
                            }
                        }
                    }
                    else if (resp != null && resp.StatusCode == HttpStatusCode.Found)
                    {
                        var location = resp.Headers["Location"];
                        if (location.Clear() != null)
                        {
                            Uri uri = new Uri(location.Clear());

                            var status = HttpUtility.ParseQueryString(uri.Query)["status"];
                            if (status.Clear() != null)
                            {
                                throw new Exception($"Login Error. Status: {status.Clear()}");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("System Error");
                    }
                }
            }

            if (stepTwoPostData.Clear() == null)
            {
                throw new Exception("Unknown Error");
            }

            _loginCookies = new CookieContainer();
            using (var loginResponse = WebClientHelper.Post(testUrl, stepTwoPostData, _loginCookies))
            {
                var httpLoginResponse = loginResponse as HttpWebResponse;

                if (!httpLoginResponse.StatusCode.In( HttpStatusCode.Found, HttpStatusCode.OK) )
                {
                    throw new Exception("Invalid Status Code");
                }

                if (!httpLoginResponse.ResponseUri.ToString().StartsWith(testUrl, StringComparison.CurrentCultureIgnoreCase))
                {
                    throw new Exception("Invalid Response Uri");
                }

                //get the sessionId 
                using (var res = WebClientHelper.Get(httpLoginResponse.ResponseUri.ToString(), null, _loginCookies))
                {
                    using (StreamReader reader = new StreamReader(res.GetResponseStream()))
                    {
                        var alltext = reader.ReadToEnd().ToString();
                        foreach (var cookie in _loginCookies.GetCookies(new Uri(testUrl)))
                        {
                            var txt = cookie.ToString();
                        }
                    }
                }
            }

        }
    }
}
