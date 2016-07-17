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

        ResponseMessage Log(Exception ex)
        {
            try
            {
                Logger.Error(ex);
                return null;
            }
            catch (Exception xx)
            {
                return new ResponseMessage
                {
                    Category = ErrorCategory.Technical,
                    Code = null,
                    Description = xx.Message,
                    Severity = ErrorSeverity.Error
                };
            }
        }
        public Tuple<R, ResponseMessage> HandleException<R>(Func<R> action) where R : class
        {
            try
            {
                return new Tuple<R, ResponseMessage> ( action(), null );
            }
            catch (Exception ex) when ( BusinessExceptions.FirstOrDefault( t => t.IsInstanceOfType(ex)) != null )
            {
                var r = Log(ex);
                if ( r != null ) { return new Tuple<R, ResponseMessage>(null, r); }
                return new Tuple<R, ResponseMessage>( null,
                    new ResponseMessage
                    {
                        Category = ErrorCategory.Business,
                        Code = ExceptionCodeResolver != null ? ExceptionCodeResolver(ex) : null,
                        Description = ex.Message,
                        Severity = ErrorSeverity.Error
                    }
                );
            }
            catch (Exception ex)
            {
                var r = Log(ex);
                if (r != null) { return new Tuple<R, ResponseMessage>(null, r); }
                return new Tuple<R, ResponseMessage>(null,
                    new ResponseMessage
                    {
                        Category = ErrorCategory.Technical,
                        Description = ex.Message,
                        Severity = ErrorSeverity.Error
                    }
                );
            }
        }

    }
}
