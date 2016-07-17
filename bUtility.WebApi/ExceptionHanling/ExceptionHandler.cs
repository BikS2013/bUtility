using bUtility.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility
{
    public class ExceptionHandler 
    {
        readonly ILogger Logger;
        readonly Type[] BusinessExceptions;
        readonly Func<Exception, string> ExceptionCodeResolver;
        public ExceptionHandler(ILogger logger, Func<Exception, string> exceptionCodeResolver,  params Type[] businessExceptionTypes)
        {
            Logger = logger;
            ExceptionCodeResolver = exceptionCodeResolver;
            BusinessExceptions = businessExceptionTypes ?? new Type[] { };
        }

        Response<R> Log<R>(Exception ex) where R : class
        {
            try
            {
                Logger.Error(ex);
                return null;
            }
            catch (Exception xx)
            {
                return new Response<R>
                {
                    Exception = new ResponseMessage
                    {
                        Category = ErrorCategory.Technical,
                        Code = null,
                        Description = xx.Message,
                        Severity = ErrorSeverity.Error
                    }
                };
            }
        }
        public Response<R> HandleException<R>(Func<Response<R>> action) where R : class
        {
            try
            {
                return action();
            }
            catch (Exception ex) when ( BusinessExceptions.FirstOrDefault( t => t.IsInstanceOfType(ex)) != null )
            {
                var r = Log<R>(ex);
                if ( r != null ) { return r; }
                return new Response<R>
                {
                    Exception =
                    new ResponseMessage
                    {
                        Category = ErrorCategory.Business,
                        Code = ExceptionCodeResolver != null ? ExceptionCodeResolver(ex) : null,
                        Description = ex.Message,
                        Severity = ErrorSeverity.Error
                    }
                };
            }
            catch (Exception ex)
            {
                var r = Log<R>(ex);
                if (r != null) { return r; }
                return new Response<R>
                {
                    Exception =
                    new ResponseMessage
                    {
                        Category = ErrorCategory.Technical,
                        Description = ex.Message,
                        Severity = ErrorSeverity.Error
                    }
                };
            }
        }

    }
}
