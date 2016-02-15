using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts.Configuration
{
    [ConfigurationCollection(typeof(AudienceUriElement), AddItemName = "uri")]
    public class AudienceUris : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AudienceUriElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AudienceUriElement)element).Value;
        }
    }
}
