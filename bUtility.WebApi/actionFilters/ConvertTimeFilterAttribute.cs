using bUtility.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace bUtility.WebApi
{
    public class ConvertTimeFilterAttribute : ActionFilterAttribute
    {
        readonly bool ConvertRequest;
        readonly bool ConvertResponse;

        public ConvertTimeFilterAttribute(bool convertRequest = true, bool convertResponse = true)
        {
            ConvertRequest = convertRequest;
            ConvertResponse = convertResponse;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (ConvertRequest && actionContext?.ActionArguments?.Count > 0)
            {
                var request = actionContext.ActionArguments.Values.First();
                var payload = request.GetValue("Payload");
                payload.ToLocalTime();
            }
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (ConvertResponse && actionExecutedContext?.Response?.Content is ObjectContent)
            {
                var obj = actionExecutedContext.Response.Content as ObjectContent;
                var response = obj.Value?.GetValue("Payload");
                response.ToUniversalTime();
            }
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
