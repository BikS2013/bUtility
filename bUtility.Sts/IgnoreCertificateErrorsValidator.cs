using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts
{
    internal class IgnoreCertificateErrorsValidator : X509CertificateValidator
    {
        public IgnoreCertificateErrorsValidator()
            : base()
        {

        }
        public override void LoadCustomConfiguration(System.Xml.XmlNodeList nodelist)
        {
            base.LoadCustomConfiguration(nodelist);
        }

        public override void Validate(System.Security.Cryptography.X509Certificates.X509Certificate2 certificate)
        {
            // We avoid validating the certificate 
        }
    }
}
