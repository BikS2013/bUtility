using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace bUtility
{

    public enum ErrorSeverity { Warning, Error, Info }
    public enum ErrorCategory { Business, Communication, Technical, Security }

    [DataContract]
    public class ResponseMessage
    {
        public ResponseMessage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "code")]
        public string Code { get; set; }
        [DataMember(Name = "desc")]
        public string Description { get; set; }
        [DataMember(Name = "sev")]
        public ErrorSeverity Severity { get; set; }
        [DataMember(Name = "cat")]
        public ErrorCategory Category { get; set; }

        public override string ToString()
        {
            return $"{this.Severity} {this.Category} {this.Code} ({this.Description})";
        }
    }
}
