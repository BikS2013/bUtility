using bUtility.Sts.ApiClient.Controllers;
using bUtility.WebApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
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
        static void handleException(Exception ex, EventLogEntryType type)
        {

        }

        X509Certificate2 getCertificate()
        {
            //το GetType().Assembly δεν δουλεύει (γιατί γίνεται build σε run-time)
            var localCert = typeof(IndexController).Assembly.GetCertificate("bUtility.Sts.ApiClient.api.model.local.pfx", "123456");
            return localCert;

        }
        X509Certificate2 getIssuerCertificate()
        {
            //το GetType().Assembly δεν δουλεύει (γιατί γίνεται build σε run-time)
            var localCert = typeof(IndexController).Assembly.GetCertificate("bUtility.Sts.ApiClient.issuer.model.local.cer");
            return localCert;

        }
        SecurityTokenDescriptor getTokenDescriptor()
        {
            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                TokenIssuerName = "self",
                AppliesToAddress = "https://localhost",
                Lifetime = new Lifetime(now, now.AddMinutes(10)),
                SigningCredentials = new X509SigningCredentials(getCertificate())
            };
            return tokenDescriptor;
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            bUtility.Sts.JWTSecurityTokenHandlerFix.AddIssuerKey(getIssuerCertificate());

            GlobalConfiguration.Configure((httpConfiguration) =>
            {
                //httpConfiguration.MessageHandlers.Add(new bUtility.WebApi.StsPostHandler("identityToken", getTokenDescriptor));
                //httpConfiguration.MessageHandlers.Add(new bUtility.WebApi.AuthenticationHandler("identityToken", getTokenDescriptor, exceptionHandler: handleException));
                httpConfiguration.Filters.Add(new bUtility.WebApi.RequireHttpsAttribute());

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