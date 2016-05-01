using bUtility.Sts;
using System;
using System.Collections.Generic;
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
using System.Xml;

namespace bUtility.WebApi
{
    public class StsPostHandler: DelegatingHandler
    {
        private readonly string _cookieName = null;
        private readonly Func<SecurityTokenDescriptor> _getTokenDescriptor = null;
        public StsPostHandler(string cookieName, Func<SecurityTokenDescriptor> getTokenDescriptor )
        {
            _cookieName = cookieName;
            _getTokenDescriptor = getTokenDescriptor;
        }

        static string getToken( string wResult)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(wResult);

            return doc?.DocumentElement?.ChildNodes[0]?.ChildNodes[2]?.InnerXml;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.GetCookies(_cookieName).FirstOrDefault() == null)
            {
                var content = request.Content.ReadAsFormDataAsync();
                var wResult = content?.Result["wresult"];
                if (wResult != null)
                {
                    var token = getToken(wResult);

                    var principal = token.GetPrincipal();
                    if (principal != null)
                    {
                        var d = _getTokenDescriptor();
                        var p = principal.Identities.FirstOrDefault().GetJwtToken(d);
                        return SendRedirectResponse(request, p);
                    }
                }
            }
            return base.SendAsync(request, cancellationToken);
        }


        private Task<HttpResponseMessage> SendRedirectResponse(HttpRequestMessage request, string actualToken)
        {
            var response = request.CreateResponse(HttpStatusCode.Found);
            response.Content = new StringContent("identity ok", Encoding.UTF8, "text/html");
            UriBuilder uri = new UriBuilder(request.RequestUri);

            var c = new CookieHeaderValue(_cookieName, actualToken);
            c.Expires = DateTime.Now.AddDays(1);
            c.Domain = uri.Host;
            c.Path = "/";
            c.Secure = true;
            c.HttpOnly = true; 
            response.Headers.AddCookies( new[] { c } );
            response.Headers.Location = uri.Uri;


            return Task.FromResult(response);
        }

    }
}
