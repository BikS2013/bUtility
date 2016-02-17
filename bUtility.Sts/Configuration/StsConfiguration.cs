using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts.Configuration
{
    public class StsConfiguration<T> : ConfigurationSection where T : ConfigurationElement, IRelyingParty, new()
    {
        [ConfigurationProperty("relyingParties")]
        public RelyingParties<T> RelyingParties
        {
            get { return base["relyingParties"] as RelyingParties<T>; }
        }

        public static StsConfiguration<T> Current
        {
            get
            {
                return ConfigurationManager.GetSection("bUtility.Sts") as StsConfiguration<T>;
            }
        }

    }
    public class StsConfiguration : StsConfiguration<RelyingParty>
    {
        public static StsConfiguration Current
        {
            get
            {
                return ConfigurationManager.GetSection("bUtility.Sts") as StsConfiguration;
            }
        }

    }
}
