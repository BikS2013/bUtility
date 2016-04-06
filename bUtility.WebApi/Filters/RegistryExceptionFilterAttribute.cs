using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace bUtility.WebApi
{
    public class RegistryExceptionFilterAttribute: ExceptionFilterAttribute
    {
        RegistryExceptionLog Logger;
        public RegistryExceptionFilterAttribute( RegistryExceptionLog logger)
        {
            Logger = logger;
        }
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            Logger.ToEventLog(actionExecutedContext.Exception);
            base.OnException(actionExecutedContext);
        }
    }
}
