using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility
{
    public class ExceptionHandler<T> where T : Exception
    {
        readonly Action<Exception> ToEventLogAction;
        public ExceptionHandler(Action<Exception> toEventLogAction)
        {
            ToEventLogAction = toEventLogAction;
        }

        public Response<R> HandleException<R>(Func<Response<R>> action) where R : class
        {
            try
            {
                return action();
            }
            catch (T ex)
            {
                if (ToEventLogAction != null)
                {
                    ToEventLogAction(ex);
                }
                return new Response<R>
                {
                    Exception =
                    new ResponseMessage
                    {
                        Category = ErrorCategory.Business,
                        Description = ex.Message,
                        Severity = ErrorSeverity.Error
                    }
                };
            }
            catch (Exception ex)
            {
                if (ToEventLogAction != null)
                {
                    ToEventLogAction(ex);
                }
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
