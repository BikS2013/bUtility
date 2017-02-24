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
        public ExceptionHandler(ILogger logger, Func<Exception, string> exceptionCodeResolver, params Type[] businessExceptionTypes)
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

        Tuple<R, ResponseMessage> ResolveException<R>(Exception ex, R nullValue)
        {
            if (ex == null) return null;
            var r = Log(ex);
            if (r != null) { return new Tuple<R, ResponseMessage>(nullValue, r); }


            return new Tuple<R, ResponseMessage>(nullValue,
                    new ResponseMessage
                    {
                        Category = BusinessExceptions.FirstOrDefault(t => t.IsInstanceOfType(ex)) != null ? ErrorCategory.Business : ErrorCategory.Technical,
                        Code = ExceptionCodeResolver != null ? ExceptionCodeResolver(ex) : null,
                        Description = ex.Message,
                        Severity = ErrorSeverity.Error
                    }
                );
        }

        public Tuple<R, ResponseMessage> HandleException<R>(Func<R> action) where R : class
        {
            return HandleException(action, null);
        }

        /// <summary>
        /// This version can be used when R is value type i.e. int, bool etc
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="action"></param>
        /// <param name="nullValue"></param>
        /// <returns></returns>
        public Tuple<R, ResponseMessage> HandleException<R>(Func<R> action, R nullValue)
        {
            try
            {
                return new Tuple<R, ResponseMessage>(action(), null);
            }
            catch (Exception ex)
            {
                return ResolveException(ex, nullValue);
            }
        }

        public Tuple<R, ResponseMessage> HandleException<T, R>(Func<T, R> action, T input) where R: class
        {
            return HandleException(action, input, null);
        }

        public Tuple<R, ResponseMessage> HandleException<T, R>(Func<T, R> action, T input, R nullValue)
        {
            try
            {
                return new Tuple<R, ResponseMessage>(action(input), null);
            }
            catch (Exception ex)
            {
                return ResolveException(ex, nullValue);
            }
        }

    }
}
