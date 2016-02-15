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
        [DataMember(Name = "body")]
        public string Body { get; set; }
        [DataMember(Name = "isBodyHtml")]
        public bool IsBodyHtml { get; set; }
        [DataMember(Name = "subject")]
        public string Subject { get; set; }
        [DataMember(Name = "mailTo")]
        public List<string> MailTo { get; set; }
        [DataMember(Name = "mailCC")]
        public List<string> MailCC { get; set; }
        [DataMember(Name = "mailBCC")]
        public List<string> MailBCC { get; set; }
        [DataMember(Name = "attachments")]
        public List<string> Attachments { get; set; }
        [DataMember(Name = "htmlResources")]
        public List<string> HtmlResources { get; set; }
    }
}
