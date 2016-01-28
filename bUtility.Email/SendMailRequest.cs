using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace bUtility
{
    [DataContract]
    public class SendMailRequest
    {
        [DataMember(Name = "userId")]
        public string UserID { get; set; }

        [DataMember(Name = "body")]
        public string Body { get; set; }

        [DataMember(Name = "subject")]
        public string Subject { get; set; }

        [DataMember(Name = "mailTo")]
        public List<string> MailTo { get; set; }

        [DataMember(Name = "mailCc")]
        public List<string> MailCC { get; set; }

        [DataMember(Name = "mailBcc")]
        public List<string> MailBCC { get; set; }

        [DataMember(Name = "attachments")]
        public List<string> Attachments { get; set; }
    }
}
