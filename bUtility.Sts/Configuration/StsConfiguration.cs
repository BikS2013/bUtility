using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts.Configuration
{
    public class StsConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("relyingParties")]
        public RelyingParties RelyingParties
        {
            get { return base["relyingParties"] as RelyingParties; }
        }

        //[ConfigurationProperty("issue")]
        //public Issue Issue
        //{
        //    get { return base["issue"] as Issue; }
        //}

        public static StsConfiguration Current
        {
            get
            {
                return ConfigurationManager.GetSection("bUtility.Sts") as StsConfiguration;
            }
        }

    }
}
