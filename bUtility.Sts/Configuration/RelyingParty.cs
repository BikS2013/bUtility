using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts.Configuration
{
    public class RelyingParty : ConfigurationElement, IRelyingParty
    {

        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get
            {
                return base["name"] as string;
            }
        }

        /// <summary>
        /// Token lifespan in minutes
        /// </summary>
        [ConfigurationProperty("tokenLifeTime")]
        public long TokenLifeTime
        {
            get
            {
                return (long)base["tokenLifeTime"];
            }
        }

        [ConfigurationProperty("redirectUrl")]
        public string RedirectUrl
        {
            get
            {
                return base["redirectUrl"] as string;
            }
        }

        [ConfigurationProperty("realm")]
        public string Realm
        {
            get
            {
                return base["realm"] as string;
            }
        }
        /// <summary>
        /// Used as the authentication method for the SAML token
        /// </summary>
        [ConfigurationProperty("authenticationUrl")]
        public string AuthenticationUrl
        {
            get
            {
                return base["authenticationUrl"] as string;
            }
        }


        [ConfigurationProperty("issuerName")]
        public string IssuerName
        {
            get
            {
                return base["issuerName"] as string;
            }
        }

        /// <summary>
        /// Token Type
        /// </summary>
        [ConfigurationProperty("tokenType")]
        public string TokenType
        {
            get
            {
                return base["tokenType"] as string;
            }
        }

        [ConfigurationProperty("encryptingCertificate")]
        public CertificateReferenceElement EncryptingCertificate
        {
            get
            {
                return base["encryptingCertificate"] as CertificateReferenceElement;
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
    }
}
