using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace bUtility.WebApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext != null && actionExecutedContext.Response != null && actionExecutedContext.Response.Headers != null)
            {
                actionExecutedContext.Response.Headers.CacheControl =
                    new System.Net.Http.Headers.CacheControlHeaderValue
                    {
                        NoCache = true,
                        NoStore = true
                    };
            }
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
