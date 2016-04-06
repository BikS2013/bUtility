using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace bUtility.Sts.ApiClient.Controllers
{
    public class IndexController : ApiController
    {
        [HttpGet, HttpPost]
        public HttpResponseMessage Get()
        {
            if (Request.Method == HttpMethod.Post)
            {
                var rresponse = Request.CreateResponse(HttpStatusCode.Redirect);
                rresponse.Headers.Location = new Uri(Request.RequestUri.AbsoluteUri);
                return rresponse;
            }
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            response.Content = new StringContent("hi");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}
