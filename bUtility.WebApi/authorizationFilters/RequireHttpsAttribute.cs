using bUtility.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace bUtility.WebApi
{
    public class RequireHttpsAttribute:AuthorizationFilterAttribute
    {
        readonly ILogger Logger; 
        public RequireHttpsAttribute(ILogger logger)
        {
            Logger = logger; 
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            if (request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                Logger.Error(new Exception($"Request Uri:{request.RequestUri.AbsoluteUri}, method:{request.Method.Method} is not https."));
                string message = "Https is required";
                if (request.Method.Method == "GET")
                {
                    actionContext.Response = request.CreateResponse(HttpStatusCode.Found);
                    actionContext.Response.Content = new StringContent(message, Encoding.UTF8, "text/html");
                    UriBuilder newUri = new UriBuilder(request.RequestUri);
                    newUri.Scheme = Uri.UriSchemeHttps;
                    newUri.Port = 443;
                    actionContext.Response.Headers.Location = newUri.Uri;
                }
                else
                {
                    actionContext.Response = request.CreateResponse(HttpStatusCode.NotFound);
                    actionContext.Response.Content = new StringContent(message, Encoding.UTF8, "text/html");
                }
            }
        }
    }
}
