using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace bUtility.Sts.ApiClient.Controllers
{
    public class IndexController : ApiController
    {
        static string indexContent = null;

        static string replaceFromAppSettings(string content, string token, string appSettingsKey)
        {
            if (System.Configuration.ConfigurationManager.AppSettings[appSettingsKey] != null)
            {
                return content.Replace(token, System.Configuration.ConfigurationManager.AppSettings[appSettingsKey]);
            }
            return content;
        }

        static object contentLock = new object();

        static string readContent()
        {
            string content = null;
            var path = HttpRuntime.AppDomainAppPath;
            using (FileStream fs = new FileStream(path + "Index.html", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    content = sr.ReadToEnd();
                }
            }
            content = replaceFromAppSettings(content, "[basehRef]", "basehRef");
            content = replaceFromAppSettings(content, "[libhRef]", "libhRef");
            //content = replaceFromAppSettings(content, "[contextPath]", "contextPath");
            return content;
        }
        static string getIndexContent()
        {
            if (indexContent == null)
            {
                lock (contentLock)
                {
                    if (indexContent == null)
                    {
                        indexContent = readContent();
                    }
                }
            }
            return indexContent;
        }

        HttpResponseMessage getResponse(string content)
        {
            var response = new HttpResponseMessage
            {
                Content = new StringContent(content)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }


        [HttpGet, HttpPost]
        public HttpResponseMessage Get()
        {
            if (Request.Method == HttpMethod.Post)
            {
                var rresponse = Request.CreateResponse(HttpStatusCode.Redirect);
                rresponse.Headers.Location = new Uri(Request.RequestUri.AbsoluteUri);
                return rresponse;
            }
            var content = readContent();

            return getResponse(content);


            //var response = new HttpResponseMessage();
            //response.StatusCode = HttpStatusCode.OK;
            //response.Content = new StringContent("hi");
            //response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            //return response;
        }
    }
}
