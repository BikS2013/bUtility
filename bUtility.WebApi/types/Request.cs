using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace bUtility
{
    public abstract class Request<T>: IRequest where T :class
    {
        [DataMember(Name = "header")]
        public RequestHeader Header { get; set; }

        public object Data { get { return Payload; } }

        [DataMember(Name = "payload")]
        public T Payload { get; set; }

        public abstract string UserID { get; set; }
    }
}
