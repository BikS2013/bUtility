using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace bUtility.Sts.Configuration
{
    public class Simple : ConfigurationElement
    {
        public string Value { get; private set; }

        protected override void DeserializeElement(XmlReader reader, bool s)
        {
            Value = reader.ReadElementContentAs(typeof(string), null) as string;
        }

        public override string ToString()
        {
            return this.Value;
        }
    }
}
