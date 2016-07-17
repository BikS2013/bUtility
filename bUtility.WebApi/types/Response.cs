using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace bUtility
{
    [DataContract]
    public class Response<T>: IExceptionContainer where T : class
    {
        [DataMember(Name = "payload")]
        public T Payload { get; set; }

        private ResponseMessage _exception;
        [DataMember(Name = "exception")]
        public ResponseMessage Exception
        {
            get { return this._exception; }
            set
            {
                if (value != null && value.Severity != ErrorSeverity.Error)
                {
                    throw new ApplicationException("Invalid use of Exception property.");
                }
                this._exception = value;
            }
        }

        [DataMember(Name = "messages")]
        public ICollection<ResponseMessage> Messages { get; set; }

        [DataMember(Name = "executionTime")]
        public decimal ExecutionTime { get; set; }

        public void AddMessage(string message)
        {
            if (Messages == null) Messages = new List<ResponseMessage>();
            Messages.Add(new ResponseMessage() { Description = message });
        }
        public void AddInfo(string code, string message)
        {
            if (Messages == null) Messages = new List<ResponseMessage>();
            Messages.Add(new ResponseMessage() { Category = ErrorCategory.Business, Severity = ErrorSeverity.Info, Code = code, Description = message });
        }
    }
}
