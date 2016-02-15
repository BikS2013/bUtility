using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts.Configuration
{
    [ConfigurationCollection(typeof(Actor), AddItemName = "actor")]
    public class Actors : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Actor();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Actor)element).Url.Value;
        }
    }
}
