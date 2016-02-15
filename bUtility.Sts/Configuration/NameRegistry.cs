using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts.Configuration
{
    public class NameRegistry : ConfigurationElement
    {
        [ConfigurationProperty("rootCertificates")]
        public RootCertificates RootCertificates
        {
            get
            {
                return base["rootCertificates"] as RootCertificates;
            }
        }
    }
}
