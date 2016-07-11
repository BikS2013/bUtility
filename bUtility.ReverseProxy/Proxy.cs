using bUtility.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace bUtility.ReverseProxy
{
    public class ReverseProxyHandler : DelegatingHandler
    {
        public class UtilityClient : HttpClient
        {
            public UtilityClient()
                : base()
            {
            }
            public UtilityClient(HttpMessageHandler messageHandler)
                : base(messageHandler)
            {

            }
            public System.Threading.Tasks.Task<HttpResponseMessage> NbgSendAsync(HttpRequestMessage request, 
                CancellationToken cancellationToken, string authHeaderSchema, string authToken)
            {
                var httpFriendlyToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(authToken));
                request.Headers.Authorization = new AuthenticationHeaderValue(authHeaderSchema, httpFriendlyToken);
                return base.SendAsync(request, cancellationToken);
            }
        }

        string WebApiSourceUrl { get; set; }
        string WebApiDestinationUrl { get; set; }
        Action<UtilityClient> PrepareApiCall { get; set; }
        Action<HttpResponseMessage> PrepareResponse { get; set; }
        public ReverseProxyHandler(string webApiSourceUrl, string webApiDestinationUrl, Action<UtilityClient> prepareApiCall, Action<HttpResponseMessage> prepareResponse)
        {
            if (string.IsNullOrEmpty(webApiSourceUrl))
                throw new ArgumentNullException("webApiSourceUrl");
            if (string.IsNullOrEmpty(webApiDestinationUrl))
                throw new ArgumentNullException("webApiDestinationUrl");
            WebApiSourceUrl = webApiSourceUrl.ToLowerInvariant();
            WebApiDestinationUrl = webApiDestinationUrl.ToLowerInvariant();
            PrepareApiCall = prepareApiCall;
            PrepareResponse = prepareResponse;
        }
        bool RemoteCertificateValidationCallback(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        protected override async System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            try
            {
                UriBuilder forwardUri = new UriBuilder(request.RequestUri);
                //send it on to the requested URL

                var apiRequest = new HttpRequestMessage(request.Method, 
                    new Uri(forwardUri.Uri.ToString().ToLowerInvariant().Replace(WebApiSourceUrl, WebApiDestinationUrl)));

                apiRequest.Version = request.Version;
                var postData = await request.Content.ReadAsByteArrayAsync();
                apiRequest.Content = new ByteArrayContent(postData);
                apiRequest.Content.Headers.ContentType = request.Content.Headers.ContentType;

                using (WebRequestHandler messageHandler = new WebRequestHandler())
                {
                    messageHandler.ServerCertificateValidationCallback += RemoteCertificateValidationCallback;
                    UtilityClient client = new UtilityClient(messageHandler);

                    PrepareApiCall?.Invoke(client);
                    //populate UtilityClient Headers based on claims, or other headers


                    var apiResponse = await client.SendAsync(apiRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
                    var respData = await apiResponse.Content.ReadAsByteArrayAsync();

                    var fResponse = request.CreateResponse();
                    fResponse.Content = new ByteArrayContent(respData);
                    fResponse.Content.Headers.ContentType = apiResponse.Content.Headers.ContentType;
                    fResponse.Content.Headers.ContentDisposition = apiResponse.Content.Headers.ContentDisposition;
                    fResponse.ReasonPhrase = apiResponse.ReasonPhrase;
                    fResponse.StatusCode = apiResponse.StatusCode;
                    fResponse.Version = apiResponse.Version;

                    //populate fResponse Headers based on apiResponse Headers

                    if (fResponse.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        //we have a 401 in api. Clear the local (in memory AND sql security token cache) 
                        //and send the 401 to the browser with clear FedAuth cookie. 
                        //The browser will refresh and will call (another perhaps) web server with no cookies, 
                        //but for html wif will redirect the browser to sts
                        System.IdentityModel.Services.FederatedAuthentication.SessionAuthenticationModule.DeleteSessionTokenCookie();
                    }
                    return fResponse;
                }
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                throw;
            }
        }

        private string GetBootstrapContextToken()
        {

            var context = ClaimsPrincipal.Current?.Identities?.FirstOrDefault()?.BootstrapContext as BootstrapContext;
            if (context != null)
            {
                var token = context.SecurityToken;
                if (context.Token != null) return context.Token;

                StringBuilder output = new StringBuilder(128);
                context.SecurityTokenHandler.WriteToken(new XmlTextWriter(new StringWriter(output)), token);
                return output.ToString();
            }
            return null;
        }
    }
}
