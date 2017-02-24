using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.WebApi
{
    public class CallHandler
    {
        protected readonly Action<object> BeforeCall;
        protected readonly Action<object> AfterCall;
        protected readonly ExceptionHandler ExceptionHandler;

        public CallHandler(ExceptionHandler exceptionHandler, Action<object> actionOnRequestBeforeCall = null, Action<object> actionOnResponseAfterCall = null)
        {
            ExceptionHandler = exceptionHandler;
            BeforeCall = actionOnRequestBeforeCall;
            AfterCall = actionOnResponseAfterCall;
        }
        
        public Response<R> Handle<T, R>(Func<T, R> call, Request<T> request, Action<Request<T>> actionOnRequestBeforeCall = null, Action<Response<R>> actionOnResponseAfterCall = null) where T : class where R : class
        {
            (actionOnRequestBeforeCall ?? BeforeCall)?.Invoke(request);
            var tuple = ExceptionHandler.HandleException(call, request?.Payload);
            var resp = tuple.ToResponse();
            (actionOnResponseAfterCall ?? AfterCall)?.Invoke(resp);
            return resp;
        }

        public Response<R> Handle<R>(Func<R> call, Request<EmptyRequest> request, Action<Request<EmptyRequest>> actionOnRequestBeforeCall = null, Action<Response<R>> actionOnResponseAfterCall = null) where R : class
        {
            (actionOnRequestBeforeCall ?? BeforeCall)?.Invoke(request);
            var tuple = ExceptionHandler.HandleException(call);
            var resp = tuple.ToResponse();
            (actionOnResponseAfterCall ?? AfterCall)?.Invoke(resp);
            return resp;
        }
    }
}
