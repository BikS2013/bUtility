using bUtility.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace bUtility.WebApi
{
    public class ResponseToUtcFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext?.Response?.Content is ObjectContent)
            {
                var obj = actionExecutedContext.Response.Content as ObjectContent;
                var response = obj.Value?.GetValue("Payload");
                response.ToUniversalTime();
            }
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
