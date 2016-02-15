using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts.Configuration
{
    [ConfigurationCollection(typeof(CertificateReferenceElement), AddItemName = "rootCertificate")]
    public class RootCertificates :
        ConfigurationElementCollection,
        IEnumerable<X509Certificate2>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CertificateReferenceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var cert = (CertificateReferenceElement)element;
            return string.Format(
                "{0}-{1}-{2}-{3}",
                cert.StoreLocation,
                cert.StoreName,
                cert.X509FindType,
                cert.FindValue);
        }

        protected override void PostDeserialize()
        {
            base.PostDeserialize();

            this._certificates.AddRange(this.
                Cast<CertificateReferenceElement>().
                Select<CertificateReferenceElement, X509Certificate2>(element => element.GetCertificate()));
        }

        private List<X509Certificate2> _certificates = new List<X509Certificate2>();

        IEnumerator<X509Certificate2> IEnumerable<X509Certificate2>.GetEnumerator()
        {
            return _certificates.GetEnumerator();
        }
    }
}
