using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts.Configuration
{
    public class Issue : ConfigurationElement
    {
        [ConfigurationProperty("nameRegistry")]
        public NameRegistry NameRegistry
        {
            get { return base["nameRegistry"] as NameRegistry; }
        }

        [ConfigurationProperty("actors")]
        public Actors Actors
        {
            get { return base["actors"] as Actors; }
        }
    }
}
