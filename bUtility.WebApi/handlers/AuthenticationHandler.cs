using bUtility.Sts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace bUtility.WebApi
{
    public class AuthenticationHandler : DelegatingHandler
    {
        private string _cookieName = null;
        private string privateApiSegment = @"privateapi/";
        private readonly Func<SecurityTokenDescriptor> _getTokenDescriptor = null;
        private Action<Exception, EventLogEntryType> _exceptionHandler = null;
        public AuthenticationHandler(
            string cookieName,
            Func<SecurityTokenDescriptor> getTokenDescriptor,
            string privateApiSegment = @"privateapi/", 
            Action<Exception, EventLogEntryType> exceptionHandler = null)
        {
            _cookieName = cookieName;
            _getTokenDescriptor = getTokenDescriptor;
            this.privateApiSegment = privateApiSegment;
            _exceptionHandler = exceptionHandler;
        }

        private ClaimsPrincipal Anonymous
        {
            get
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "")
                    };

                var anonId = new ClaimsIdentity(claims);
                return new ClaimsPrincipal(anonId);
            }
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            try
            {
                var c = request.Headers.GetCookies(_cookieName).FirstOrDefault();
                if ( c != null)
                {
                    var principal = c[_cookieName].Value.ReadJwtToken(_getTokenDescriptor());
                    if (principal.Identity.IsAuthenticated)
                    {
                        Thread.CurrentPrincipal = principal;
                        //http://www.asp.net/web-api/overview/security/authentication-and-authorization-in-aspnet-web-api
                        if (HttpContext.Current != null)
                        {
                            HttpContext.Current.User = principal;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                if ( _exceptionHandler != null)
                {
                    _exceptionHandler(ex, EventLogEntryType.Error);
                }
            }

            if (Thread.CurrentPrincipal?.Identity?.IsAuthenticated != true )
            {
                var sLength = request?.RequestUri?.Segments?.Length ?? 0;
                if ( sLength > 3 && request.RequestUri.Segments[sLength - 3].ToLower() == privateApiSegment.ToLower())
                {
                    //in private api
                }
                else
                {
                    if ( _exceptionHandler != null)
                    {
                        _exceptionHandler(new Exception($"Unauthorized request:{request?.RequestUri?.ToString() ?? "RequestUri is Null"}"), EventLogEntryType.Warning);
                    }
                    return SendUnauthorizedResponse(request);
                }
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
