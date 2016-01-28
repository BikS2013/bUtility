using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace bUtility
{
    [DataContract]
    public class RequestHeader 
    {
        [DataMember(Name = "ID")]
        public string ID { get; set; }

        [DataMember(Name = "application")]
        public string Application { get; set; }

        [DataMember(Name = "channel")]
        public string Channel { get; set; }

        [DataMember(Name = "logitude")]
        public decimal? Longitude { get; set; }

        [DataMember(Name = "latitude")]
        public decimal? Latitude { get; set; }
    }
}
