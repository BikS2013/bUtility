using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace bUtility.WebApi
{
    public class ControllerSelector : DefaultHttpControllerSelector
    {
        public string DefaultControllerName { get; private set; }
        public string DefaultActionName { get; private set; }
        public ControllerSelector(HttpConfiguration configuration,
            string controllerName, string actionName)
              : base(configuration)
        {
            DefaultControllerName = controllerName;
            DefaultActionName = actionName;
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            HttpControllerDescriptor decriptor = null;
            try
            {
                decriptor = base.SelectController(request);
            }
            catch (HttpResponseException ex)
            {
                var code = ex.Response.StatusCode;
                if (code != HttpStatusCode.NotFound)
                    throw;
                var routeValues = request.GetRouteData().Values;
                routeValues["controller"] = DefaultControllerName ?? "Error";
                routeValues["action"] = DefaultActionName ?? "Handle404";
                decriptor = base.SelectController(request);
            }
            return decriptor;
        }

    }
}
