using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace bUtility.ReverseProxy
{
    public static class HttpExtensions
    {
        public static async Task<HttpResponseMessage> PrepareResponseAsync(this HttpRequestMessage request, HttpResponseMessage apiResponse)
        {
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

        public static bool IsValidUrl(this string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)
                   && Uri.IsWellFormedUriString(url, UriKind.Absolute);
        }
    }
}
