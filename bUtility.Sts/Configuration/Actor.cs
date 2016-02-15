using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts.Configuration
{
    public class Actor : ConfigurationElement
    {
        [ConfigurationProperty("url")]
        public Simple Url
        {
            get { return base["url"] as Simple; }
        }

        [ConfigurationProperty("issuerName")]
        public Simple IssuerName
        {
            get
            {
                return base["issuerName"] as Simple;
            }
        }

        [ConfigurationProperty("signingCertificate")]
        public CertificateReferenceElement SigningCertificate
        {
            get
            {
                return base["signingCertificate"] as CertificateReferenceElement;
            }
        }

        [ConfigurationProperty("callerIdentities")]
        public CallerIdentities CallerIdentities
        {
            get
            {
                return base["callerIdentities"] as CallerIdentities;
            }
        }

        public X509SigningCredentials SigningCredentials
        {
            get
            {
                _signingCertificate = _signingCertificate == null ?
                    SigningCertificate.GetCertificate() :
                    _signingCertificate;

                return new X509SigningCredentials(_signingCertificate);
            }
        }

        private X509Certificate2 _signingCertificate = null;
    }
}
