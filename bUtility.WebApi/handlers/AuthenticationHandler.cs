using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Services;
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
        private string privateApiSegment = @"privateapi/";
        private Action<Exception, EventLogEntryType> _exceptionHandler = null;
        public AuthenticationHandler(string privateApiSegment = @"privateapi/", 
            Action<Exception, EventLogEntryType> exceptionHandler = null)
        {
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
                var authZ = request.Headers.Authorization;

                if (authZ != null)
                {
                    var principal = AuthenticateAuthorizationHeader(authZ.Scheme, authZ.Parameter);

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
                }
            }

            return base.SendAsync(request, cancellationToken);
            //return SendUnauthorizedResponse(request);
        }

        private Task<HttpResponseMessage> SendUnauthorizedResponse(HttpRequestMessage request)
        {
            var unauthorizedResponse = request.CreateResponse(HttpStatusCode.Forbidden);

            unauthorizedResponse.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("SAML", ""));

            unauthorizedResponse.ReasonPhrase = "Forbidden.";

            return Task.FromResult(unauthorizedResponse);
        }
        private ClaimsPrincipal AuthenticateAuthorizationHeader(string scheme, string credential)
        {
            try
            {
                if (!string.IsNullOrEmpty(credential))
                {
                    // Get the base64 encoded token (converted that way to make it http friendly)
                    credential = Encoding.UTF8.GetString(Convert.FromBase64String(credential));
                }
                var handlers = FederatedAuthentication.FederationConfiguration.IdentityConfiguration.SecurityTokenHandlers;

                using (var textReader = new StringReader(credential))
                {
                    using (var xmlReader = new XmlTextReader(textReader))
                    {
                        var tokens = handlers.ReadToken(xmlReader);
                        if (tokens != null)
                        {
                            var identities = handlers.ValidateToken(tokens);
                            if (identities != null)
                            {
                                var principal = new ClaimsPrincipal(identities);
                                if (principal != null)
                                {
                                    return principal;
                                }
                            }
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


            return Anonymous;
        }
    }
}
