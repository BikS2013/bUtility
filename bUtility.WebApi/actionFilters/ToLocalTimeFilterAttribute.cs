using bUtility.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace bUtility.WebApi
{
    public class ToLocalTimeFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext?.ActionArguments?.Count > 0)
            {
                var request = actionContext.ActionArguments.Values.First();
                var payload = request.GetValue("Payload");
                payload.ToLocalTime();
            }
            base.OnActionExecuting(actionContext);
        }
    }
}
