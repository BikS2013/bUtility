using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bUtility.WebApi
{
    public class RequiresAuthenticationHandler : DelegatingHandler
    {
        public RequiresAuthenticationHandler()
        {
        }

        //http://www.asp.net/web-api/overview/security/authentication-and-authorization-in-aspnet-web-api
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            if (Thread.CurrentPrincipal?.Identity?.IsAuthenticated != true)
            {
                return SendUnauthorizedResponse(request);
            }

            return base.SendAsync(request, cancellationToken);
        }

        private Task<HttpResponseMessage> SendUnauthorizedResponse(HttpRequestMessage request)
        {
            var unauthorizedResponse = request.CreateResponse(HttpStatusCode.Forbidden);

            unauthorizedResponse.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("SAML", ""));

            unauthorizedResponse.ReasonPhrase = "Forbidden.";

            return Task.FromResult(unauthorizedResponse);
        }
    }
}
