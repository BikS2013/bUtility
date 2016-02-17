using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts.Configuration
{
    [ConfigurationCollection(typeof(IRelyingParty), AddItemName = "rp")]
    public class RelyingParties<T>: ConfigurationElementCollection where T : ConfigurationElement, IRelyingParty, new()
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IRelyingParty)element).Name;
        }

        public IRelyingParty FindByName(string name)
        {
            return this.
                Cast<IRelyingParty>().
                FirstOrDefault(rp => rp?.Name?.ToUpper() == name?.ToUpper());
        }
    }
}
