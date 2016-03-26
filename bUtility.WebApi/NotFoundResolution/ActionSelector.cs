using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace bUtility.WebApi
{
    public class ActionSelector : ApiControllerActionSelector
    {
        public Func<IHttpController> ControllerBuilder { get; private set; }
        public string DefaultActionName { get; private set; }
        public ActionSelector(Func<IHttpController> controllerBuilder, string defaultActionName)
        {
            ControllerBuilder = controllerBuilder;
            DefaultActionName = defaultActionName;
        }

        public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            HttpActionDescriptor decriptor = null;
            try
            {
                decriptor = base.SelectAction(controllerContext);
            }
            catch (HttpResponseException ex)
            {
                var code = ex.Response.StatusCode;
                if (code != HttpStatusCode.NotFound && code != HttpStatusCode.MethodNotAllowed)
                    throw;
                var routeData = controllerContext.RouteData;
                routeData.Values["action"] = DefaultActionName;
                IHttpController httpController = ControllerBuilder();
                controllerContext.Controller = httpController;
                controllerContext.ControllerDescriptor =
                    new HttpControllerDescriptor(controllerContext.Configuration,
                    httpController.GetType().Name, httpController.GetType());
                decriptor = base.SelectAction(controllerContext);
            }
            return decriptor;
        }
    }
}
