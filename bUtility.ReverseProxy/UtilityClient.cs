using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace bUtility.ReverseProxy
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
        public System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken, string authHeaderSchema, string authToken)
        {
            var httpFriendlyToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(authToken));
            request.Headers.Authorization = new AuthenticationHeaderValue(authHeaderSchema, httpFriendlyToken);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
