using System.Runtime.Serialization;

namespace bUtility
{
    public class EmptyRequest : IRequest
    {
        [DataMember(Name = "header")]
        public RequestHeader Header { get; set; }

        public object Data { get { return null; } }
        public string UserID { get; set; }
    }
}
