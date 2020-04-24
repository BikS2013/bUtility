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
        string WebApiSourceUrl { get; set; }
        string WebApiDestinationUrl { get; set; }
        Action<HttpRequestMessage> PrepareApiCall { get; set; }
        Action<HttpResponseMessage> PrepareResponse { get; set; }
        private readonly HttpClient Client;
        readonly ILogger Logger;
        public ReverseProxyHandler(string webApiSourceUrl, string webApiDestinationUrl, 
            HttpClient client, Action<HttpRequestMessage> prepareApiCall,
            Action<HttpResponseMessage> prepareResponse, ILogger logger)
        {
            if (string.IsNullOrEmpty(webApiSourceUrl))
                throw new ArgumentNullException("webApiSourceUrl");
            if (string.IsNullOrEmpty(webApiDestinationUrl))
                throw new ArgumentNullException("webApiDestinationUrl");
            WebApiSourceUrl = webApiSourceUrl.ToLowerInvariant();
            WebApiDestinationUrl = webApiDestinationUrl.ToLowerInvariant();
            Client = client;
            PrepareApiCall = prepareApiCall;
            PrepareResponse = prepareResponse;
            Logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Logger.Warn($"requestUri: {request.RequestUri}, method: {request.Method}");
            if (request.RequestUri.ToString().StartsWith(WebApiSourceUrl, StringComparison.InvariantCultureIgnoreCase))
            {
                try
                {
                    UriBuilder forwardUri = new UriBuilder(request.RequestUri);
                    var targetUri = new Uri(forwardUri.Uri.ToString().ToLowerInvariant()
                        .Replace(WebApiSourceUrl, WebApiDestinationUrl));
                    Logger.Warn($"targetUri: {targetUri}");

                    //send it on to the requested URL 
                    var apiRequest = new HttpRequestMessage(request.Method, targetUri);

                    apiRequest.Version = request.Version;
                    //Get methods doesn't support content-body
                    if (request.Method != HttpMethod.Get)
                    {
                        var postData = await request.Content.ReadAsByteArrayAsync();
                        apiRequest.Content = new ByteArrayContent(postData);
                        apiRequest.Content.Headers.ContentType = request.Content.Headers.ContentType;
                    }

                    PrepareApiCall(apiRequest);

                    var apiResponse = await Client.SendAsync(apiRequest, HttpCompletionOption.ResponseHeadersRead,
                        cancellationToken);
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
                        System.IdentityModel.Services.FederatedAuthentication.SessionAuthenticationModule
                            .DeleteSessionTokenCookie();
                    }

                    return fResponse;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw;
                }
            }
            else
            {
                return await base.SendAsync(request, cancellationToken);
            }
        }
    }
}
