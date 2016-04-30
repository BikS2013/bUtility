using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bUtility.WebApi
{
    public class ThrottlingHandler : DelegatingHandler
    {
        private readonly int _maxConcurrency = 0;
        private readonly int _maxPerUserConcurrency = 0;
        private readonly Dictionary<string, int> _perUserCurrentConcurrency = null;
        private readonly Action<Exception> _exceptionHandler = null; 
        private int _currentConcurrency = 0;


        public ThrottlingHandler(int maxConcurrency, int maxPerUserConcurrency = 0, Action<Exception> exceptionHandler = null)
        {
            if (maxConcurrency < 0)
                throw new ArgumentException("Negative concurrency detected.", "maxConcurrency");

            if (maxPerUserConcurrency < 0)
                throw new ArgumentException("Negative concurrency detected.", "maxPerUserConcurrency");

            _exceptionHandler = exceptionHandler;
            _maxConcurrency = maxConcurrency;
            _maxPerUserConcurrency = maxPerUserConcurrency;

            _currentConcurrency = 0;
            _perUserCurrentConcurrency = new Dictionary<string, int>();
        }

        public bool IsConcurrencyThrottlingEnabled { get { return _maxConcurrency > 0; } }
        public bool IsPerUserConcurrencyThrottlingEnabled
        {
            get
            {
                return _maxPerUserConcurrency > 0 &&
                    Thread.CurrentPrincipal != null &&
                    Thread.CurrentPrincipal.Identity != null &&
                    Thread.CurrentPrincipal.Identity.IsAuthenticated;
            }
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                int currentConcurrency = IncrementCurrentConcurrency();
                int userCurrentConcurrency = IncrementUserCurrentConcurrency();

                if (currentConcurrency > _maxConcurrency)
                {
                    _exceptionHandler(new ThrottlingException(
                        $"Global Throttling Limit Exceeded. CurrentConcurrency: {currentConcurrency} MaxConcurrency: {_maxConcurrency}"
                        ));
                    return QuotaExceededResponse(request);
                }
                else if (userCurrentConcurrency > _maxPerUserConcurrency)
                {
                    _exceptionHandler(new ThrottlingException(
                        $"User Throttling Limit Exceeded. CurrentConcurrency: {userCurrentConcurrency} MaxConcurrency: {_maxPerUserConcurrency} User: {Thread.CurrentPrincipal.Identity.Name}"
                        ));
                    return QuotaExceededResponse(request);
                }

                //no throttling required
                //this should be awaited because we need to know the end of the call to decrement the counters 
                return await base.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                _exceptionHandler(new ThrottlingException("Throttling Error", ex));
                throw;
            }
            finally
            {
                DecrementUserCurrentConcurrency();
                DecrementCurrentConcurrency();
            }
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private int IncrementCurrentConcurrency()
        {
            if (IsConcurrencyThrottlingEnabled)
                return Interlocked.Increment(ref _currentConcurrency);
            else
                return -1;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private void DecrementCurrentConcurrency()
        {
            if (IsConcurrencyThrottlingEnabled)
                Interlocked.Decrement(ref _currentConcurrency);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private int IncrementUserCurrentConcurrency()
        {
            if (IsPerUserConcurrencyThrottlingEnabled)
                lock (_perUserCurrentConcurrency)
                {
                    if (!_perUserCurrentConcurrency.ContainsKey(Thread.CurrentPrincipal.Identity.Name))
                        return _perUserCurrentConcurrency[Thread.CurrentPrincipal.Identity.Name] = 1;
                    else
                        return _perUserCurrentConcurrency[Thread.CurrentPrincipal.Identity.Name] += 1;
                }
            else
                return -1;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private void DecrementUserCurrentConcurrency()
        {
            if (IsPerUserConcurrencyThrottlingEnabled)
                lock (_perUserCurrentConcurrency)
                {
                    if ((_perUserCurrentConcurrency[Thread.CurrentPrincipal.Identity.Name] -= 1) == 0)
                        _perUserCurrentConcurrency.Remove(Thread.CurrentPrincipal.Identity.Name);
                }
        }

        private HttpResponseMessage QuotaExceededResponse(HttpRequestMessage request)
        {
            var response = request.CreateResponse((HttpStatusCode)429, "Too many requests.");
            response.Headers.RetryAfter = new System.Net.Http.Headers.RetryConditionHeaderValue(TimeSpan.FromSeconds(5)); //Typical, no real limit
            return response;
        }
    }
}
