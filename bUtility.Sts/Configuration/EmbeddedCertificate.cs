using bUtility.Reflection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts.Configuration
{
    public class EmbeddedCertificate : ConfigurationElement
    {
        [ConfigurationProperty("assemblyName")]
        public string AssemblyName
        {
            get
            {
                return base["assemblyName"] as string;
            }
        }

        [ConfigurationProperty("resourceName")]
        public string ResourceName
        {
            get
            {
                return base["resourceName"] as string;
            }
        }

        [ConfigurationProperty("password")]
        public string Password
        {
            get
            {
                return base["password"] as string;
            }
        }
    }
}
