using bUtility.Logging;
using bUtility.SecurityToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace bUtility.WebApi
{
    public class AuthenticationHeaderHandler : DelegatingHandler
    {
        ILogger Logger;
        public AuthenticationHeaderHandler(ILogger logger)
        {
            Logger = logger;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            try
            {
                var principal = request.Headers.Authorization.GetClaimsPrincipal("SAML");

                if (principal?.Identity?.IsAuthenticated == true)
                {
                    Thread.CurrentPrincipal = principal;
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.User = principal;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return base.SendAsync(request, cancellationToken);
        }

    }
}
