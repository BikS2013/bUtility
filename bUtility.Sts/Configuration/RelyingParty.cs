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
    public class RelyingParty : ConfigurationElement
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

        [ConfigurationProperty("audienceUris")]
        public AudienceUriElementCollection AudienceUris
        {
            get
            {
                return base["audienceUris"] as AudienceUriElementCollection;
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

        private X509Certificate2 _encryptingCertificate = null;
        public X509EncryptingCredentials EncryptingCredentials
        {
            get
            {
                // allow optional use of the encrypting certificate
                if (!EncryptingCertificate.ElementInformation.IsPresent) return null;
                _encryptingCertificate = _encryptingCertificate ?? EncryptingCertificate.GetCertificate();
                return new X509EncryptingCredentials(_encryptingCertificate);
            }
        }

        public X509SigningCredentials SigningCredentials
        {
            get
            {
                return new X509SigningCredentials(SigningX509Certificate);
            }
        }

        private X509Certificate2 _signingCertificate = null;
        X509Certificate2 SigningX509Certificate
        {
            get
            {
                if (!SigningCertificate.ElementInformation.IsPresent)
                    throw new Exception("SigningCertificate is not specified in the configuration! Signing is required.");
                _signingCertificate = _signingCertificate ?? SigningCertificate.GetCertificate();
                return _signingCertificate;
            }
        }
    }
}
