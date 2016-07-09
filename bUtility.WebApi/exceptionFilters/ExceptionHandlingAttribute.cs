using bUtility.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace bUtility.WebApi
{
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// if true hides the exception details from end user, creating a new response
        /// </summary>
        public bool MuteExceptions { get; set; }

        /// <summary>
        /// List of the exception types which we prefer to ignore during logging
        /// </summary>
        public Type[] MinorExceptionTypes { get; set; }


        public ExceptionHandlingAttribute( bool muteExceptions = true, Type[] minorExceptionTypes = null)
        {
            MuteExceptions = muteExceptions;
            MinorExceptionTypes = minorExceptionTypes;
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);
            var exception = actionExecutedContext.Exception;
            var statusCode = GetExceptionStatusCode(exception);
            if (MuteExceptions && statusCode.HasValue)
            {
                actionExecutedContext.Response = new HttpResponseMessage(statusCode.Value);
            }
            var minorExceptionType = MinorExceptionTypes?.FirstOrDefault(e => e.IsAssignableFrom(exception.GetType()));
            if (minorExceptionType != null)
            {
                //do nothing, ignore these exception. is minor
            }
            else if (exception is System.OperationCanceledException)
            {
                Logger.Current.Info($"Request {actionExecutedContext.Request.RequestUri } cancelled by user.");
            }
            else
            {
                Logger.Current.Error(exception);
            }
        }

        private static HttpStatusCode? GetExceptionStatusCode(Exception exception)
        {
            var mapping = _mappings.FirstOrDefault(m => m.ExceptionType.IsAssignableFrom(exception.GetType()));
            if (mapping != null)
            {
                return mapping.StatusCode;
            }

            return null;
        }

        public static void OnSerializationError(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs e)
        {
            Logger.Current.Error(e.ErrorContext.Error, $"Serializing: {e?.CurrentObject?.GetType().FullName}");
            var statusCode = GetExceptionStatusCode(e.ErrorContext.Error);
            if (statusCode.HasValue)
            {
                e.ErrorContext.Handled = true;
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.StatusCode = (int)statusCode.Value;
                HttpContext.Current.Response.End();
            }
        }

        private static readonly ExceptionMapping[] _mappings = new ExceptionMapping[]
        {
            new ExceptionMapping(typeof(System.Security.SecurityException), HttpStatusCode.Unauthorized),
            new ExceptionMapping(typeof(System.Runtime.Serialization.SerializationException), HttpStatusCode.BadRequest),
            new ExceptionMapping(typeof(Newtonsoft.Json.JsonSerializationException), HttpStatusCode.BadRequest),
            new ExceptionMapping(typeof(System.NotImplementedException), HttpStatusCode.MethodNotAllowed),
            new ExceptionMapping(typeof(System.Exception), HttpStatusCode.InternalServerError),
        };

        private class ExceptionMapping
        {
            public Type ExceptionType { get; private set; }
            public HttpStatusCode StatusCode { get; private set; }

            public ExceptionMapping(Type exceptionType, HttpStatusCode statusCode)
            {
                if (exceptionType == null)
                {
                    throw new ArgumentNullException("exceptionType");
                }
                else if (!typeof(System.Exception).IsAssignableFrom(exceptionType))
                {
                    throw new ArgumentException("exceptionType must derive from System.Exception");
                }

                this.ExceptionType = exceptionType;
                this.StatusCode = statusCode;
            }
        }
    }
}
