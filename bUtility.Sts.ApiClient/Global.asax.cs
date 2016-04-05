using bUtility.Sts.ApiClient.Controllers;
using bUtility.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Security;
using System.Web.SessionState;

namespace bUtility.Sts.ApiClient
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure((httpConfiguration) =>
            {

                httpConfiguration.Routes.MapHttpRoute(
                       name: "DefaultPage",
                       routeTemplate: "",
                        defaults: new { controller = "Index", action = "Get" }
                  );

                httpConfiguration.Routes.MapHttpRoute(
                       name: "DefaultApi",
                       routeTemplate: "api/{controller}/{action}"
                  );

                httpConfiguration.Routes.MapHttpRoute(
                    name: "Error404",
                    routeTemplate: "{*url}",
                    defaults: new { controller = "Index", action = "Get" }
                );

                httpConfiguration.Services.Replace(typeof(IHttpControllerSelector),
                    new ControllerSelector(httpConfiguration, "Index", "Get"));
                httpConfiguration.Services.Replace(typeof(IHttpActionSelector),
                    new ActionSelector(() => { return new IndexController(); },
                    "Get"));
            });

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}