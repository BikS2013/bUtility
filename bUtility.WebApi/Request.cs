using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace bUtility
{
    public class Request<T>: IUserIDProvider where T :class, IUserIDProvider
    {
        [DataMember(Name = "header")]
        public RequestHeader Header { get; set; }

        [DataMember(Name = "payload")]
        public T Payload { get; set; }

        public string UserID
        {
            get
            {
                var userIDProvider = this.Payload as IUserIDProvider;
                return userIDProvider == null ? null : userIDProvider.UserID;
            }
            set
            {
                var userIDProvider = this.Payload as IUserIDProvider;
                if (userIDProvider != null)
                    userIDProvider.UserID = value;
            }
        }
    }
}
