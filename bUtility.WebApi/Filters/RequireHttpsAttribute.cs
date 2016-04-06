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
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            if (request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                actionContext.Response = request.CreateResponse(HttpStatusCode.NotFound);
                actionContext.Response.Content = new StringContent("Please try using https instead", Encoding.UTF8);
            }
        }
    }
}
