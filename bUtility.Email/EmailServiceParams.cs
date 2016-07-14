using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility
{
    public class EmailServiceParams
    {
        //string from, string fromDisplayName, string sender, string senderDisplayName, 
        //string smtp, int port, bool enableSSL, IResourceResolver resourceResolver

        public string FromAddress { get; set; }
        public string FromDisplayName { get; set; }
        public string SenderAddress { get; set; }
        public string SenderDisplayName { get; set; }
        public string SmtpAddress { get; set; }
        public int SmtpPort { get; set; }
        public bool Ssl { get; set; }
    }
}
