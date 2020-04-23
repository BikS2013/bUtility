using bUtility.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace bUtility.ReverseProxy
{
    public class ReverseProxyHandler : DelegatingHandler
    {  
        private readonly ILogger _logger;
        private readonly ReverseProxyConfigList _reverseProxyConfigList;
        private readonly Action<HttpRequestMessage> _prepareApiCall;
        private readonly Func<HttpRequestMessage, HttpResponseMessage, Task<HttpResponseMessage>> _prepareResponseAsync;
        private readonly Dictionary<string, HttpClient> _httpClientProvider;

        public ReverseProxyHandler(ILogger logger, ReverseProxyConfigList reverseProxyConfigList,
            Action<HttpRequestMessage> prepareApiCall, 
            Func<HttpRequestMessage, HttpResponseMessage, Task<HttpResponseMessage>> prepareResponseAsync,
            Dictionary<string, HttpClient> httpClientProvider)
        {
            ValidateInput(reverseProxyConfigList, prepareApiCall, prepareResponseAsync, httpClientProvider);
            _logger = logger;
            _reverseProxyConfigList = reverseProxyConfigList;
            _prepareApiCall = prepareApiCall;
            _prepareResponseAsync = prepareResponseAsync;
            _httpClientProvider = httpClientProvider;
        }

        private void ValidateInput(ReverseProxyConfigList reverseProxyConfigList, Action<HttpRequestMessage> prepareApiCall,
            Func<HttpRequestMessage, HttpResponseMessage, Task<HttpResponseMessage>> prepareResponseAsync, Dictionary<string, HttpClient> httpClientProvider)
        {
            if (reverseProxyConfigList == null) throw new ArgumentNullException(nameof(reverseProxyConfigList));
            if (!reverseProxyConfigList.ReverseProxyConfiguration.HasAny())
                throw new ArgumentOutOfRangeException(nameof(reverseProxyConfigList.ReverseProxyConfiguration), 
                    "No Api Routes Declared");
            if (prepareApiCall == null) throw new ArgumentNullException(nameof(prepareApiCall));
            if (prepareResponseAsync == null) throw new ArgumentNullException(nameof(prepareResponseAsync));
        }
         
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _logger.Warn($"requestUri: {request.RequestUri}, method: {request.Method}");

            try
            {
                return await Execute(request, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        private async Task<HttpResponseMessage> Execute(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ReverseProxyConfig targetReverseProxy = GetTargetRouteConfiguration(request);
            Uri targetUri = GetTargetUri(request, targetReverseProxy);
            HttpClient client = _httpClientProvider.TryGetClient(targetReverseProxy.Name);
            HttpRequestMessage apiRequest = await GetApiRequestAsync(request, targetUri);
            var apiResponse = await client.SendAsync(apiRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            return await _prepareResponseAsync.Invoke(request, apiResponse);
        }

        private async Task<HttpRequestMessage> GetApiRequestAsync(HttpRequestMessage request, Uri targetUri)
        {
            var apiRequest = new HttpRequestMessage(request.Method, targetUri) { Version = request.Version };
            //Get methods doesn't support content-body
            if (request.Method != HttpMethod.Get)
            {
                var postData = await request.Content.ReadAsByteArrayAsync();
                apiRequest.Content = new ByteArrayContent(postData);
                apiRequest.Content.Headers.ContentType = request.Content.Headers.ContentType;
            }
            _prepareApiCall(apiRequest);
            return apiRequest;
        }

        #region Support   
        private ReverseProxyConfig GetTargetRouteConfiguration(HttpRequestMessage request)
        {
            return _reverseProxyConfigList.ReverseProxyConfiguration.FirstOrDefault(r =>
                request.RequestUri.ToString().ToLowerInvariant()
                    .Contains(r.SourceUri.ToLowerInvariant()));
        }

        private Uri GetTargetUri(HttpRequestMessage request, ReverseProxyConfig targetReverseProxyConfiguration)
        {
            var targetUri = new Uri(request.RequestUri.ToString()
                .ReplaceInsensitive(targetReverseProxyConfiguration.SourceUri.ToLowerInvariant(),
                    targetReverseProxyConfiguration.TargetUri.ToLowerInvariant()));
                
            _logger.Warn($"targetUri: {targetUri}");
            return targetUri;
        }
        #endregion
    }
}
