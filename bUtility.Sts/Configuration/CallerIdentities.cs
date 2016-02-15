using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts.Configuration
{
    [ConfigurationCollection(typeof(Simple), AddItemName = "callerIdentity")]
    public class CallerIdentities : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Simple();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Simple)element).Value;
        }
    }
}
