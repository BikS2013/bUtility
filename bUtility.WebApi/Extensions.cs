using bUtility.Reflection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;

namespace bUtility.WebApi
{
    public static class Extensions
    {
        public static string GetRequestPath(this HttpRequest request)
        {
            if (request.ApplicationPath.Length == 1)
                return request.Path;

            return request.Path.Substring(request.ApplicationPath.Length);
        }
        public static void GenerateExceptionResponse(this HttpActionContext actionContext, Type responsePayloadType, ResponseMessage message)
        {
            var response = responsePayloadType.GetConstructor(System.Type.EmptyTypes).Invoke(null);
            response.SetValue("Exception", message);

            actionContext.Response = actionContext.Request.CreateResponse();
            actionContext.Response.StatusCode = System.Net.HttpStatusCode.OK;
            actionContext.Response.Content = new ObjectContent(responsePayloadType, response, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
        }

        public static void GenerateExceptionResponseEnumToString(this HttpActionContext actionContext, Type responsePayloadType, ResponseMessage message)
        {
            var response = responsePayloadType.GetConstructor(System.Type.EmptyTypes).Invoke(null);
            response.SetValue("Exception", message);

            actionContext.Response = actionContext.Request.CreateResponse();
            actionContext.Response.StatusCode = System.Net.HttpStatusCode.OK;
            actionContext.Response.Content = new ObjectContent(responsePayloadType, response, new System.Net.Http.Formatting.JsonMediaTypeFormatter()
            {
                SerializerSettings = new JsonSerializerSettings
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                    Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() }
                }
            });
        }

        public static void GenerateSecurityExceptionResponse(this HttpActionContext actionContext, Type responsePayloadType, string exceptionCode, string exceptionDescription, ErrorCategory category = ErrorCategory.Security, ErrorSeverity severity = ErrorSeverity.Error)
        {
            GenerateExceptionResponse(actionContext, responsePayloadType, new ResponseMessage
            {
                Code = exceptionCode,
                Description = exceptionDescription,
                Category = category,
                Severity = severity
            });
        }
        public static void GenerateSecurityExceptionResponse(this HttpActionContext actionContext, Type responsePayloadType, string id, string exceptionCode, string exceptionDescription, ErrorCategory category = ErrorCategory.Security, ErrorSeverity severity = ErrorSeverity.Error)
        {
            GenerateExceptionResponse(actionContext, responsePayloadType, new ResponseMessage
            {
                Id = id,
                Code = exceptionCode,
                Description = exceptionDescription,
                Category = category,
                Severity = severity
            });
        }

        public static Response<R> ToResponse<R>(this Tuple<R, ResponseMessage> tuple) where R: class
        {
            if (tuple == null) return null;
            var resp = new Response<R>();
            resp.Payload = tuple.Item1;
            resp.Exception = tuple.Item2;
            return resp;
        }
    }
}
