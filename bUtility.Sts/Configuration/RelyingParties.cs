using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts.Configuration
{
    [ConfigurationCollection(typeof(RelyingParty), AddItemName = "rp")]
    public class RelyingParties : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RelyingParty();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RelyingParty)element).Name;
        }

        public RelyingParty FindByRealm(string realm)
        {
            return this.
                Cast<RelyingParty>().
                FirstOrDefault(rp =>
                    rp.Realm.Equals(realm, StringComparison.InvariantCultureIgnoreCase));
        }

        public RelyingParty FindByName(string name)
        {
            return this.
                Cast<RelyingParty>().
                FirstOrDefault(rp => rp.Name.ToUpper() == name.ToUpper());
        }

        public RelyingParty Default()
        {
            return this.
                Cast<RelyingParty>().
                FirstOrDefault();
        }
    }
}
