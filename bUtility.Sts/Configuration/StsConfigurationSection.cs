using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts.Configuration
{
    public class StsConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("relyingParties")]
        public RelyingPartyCollection RelyingParties
        {
            get { return base["relyingParties"] as RelyingPartyCollection; }
        }

        public static StsConfigurationSection Current
        {
            get
            {
                return ConfigurationManager.GetSection("bUtility.Sts") as StsConfigurationSection;
            }
        }

    }
}
