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
                string value = base["tokenType"] as string;
                switch (value)
                {
                    case "Saml11TokenProfile11": return TokenTypes.Saml11TokenProfile11;
                    case "Saml2TokenProfile11": return TokenTypes.Saml2TokenProfile11;
                    case "JsonWebToken": return TokenTypes.JsonWebToken;
                    default: return TokenTypes.Saml11TokenProfile11;
                }
            }
        }


        public bool DebugEnabled
        {
            get { return this.Debug.HasValue && this.Debug.Value; }
        }

        [ConfigurationProperty("debug", DefaultValue = false, IsRequired = false)]
        public bool? Debug
        {
            get
            {
                return base["debug"] as bool?;
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
                if (!this.EncryptingCertificate.ElementInformation.IsPresent) return null;
                this._encryptingCertificate = this._encryptingCertificate ?? GetCertificate(this.EncryptingCertificate);
                return new X509EncryptingCredentials(this._encryptingCertificate);
            }
        }

        public X509SigningCredentials SigningCredentials
        {
            get
            {
                return new X509SigningCredentials(this.SigningX509Certificate);
            }
        }

        private X509Certificate2 _signingCertificate = null;
        public X509Certificate2 SigningX509Certificate
        {
            get
            {
                if (!this.SigningCertificate.ElementInformation.IsPresent) throw new ArgumentException("SigningCertificate is not specified in the configuration! Signing is required.");
                this._signingCertificate = this._signingCertificate ?? GetCertificate(this.SigningCertificate);
                return this._signingCertificate;
            }
        }


        private static X509Certificate2 GetCertificate(CertificateReferenceElement reference)
        {
            if (reference != null)
            {
                return CertificateHelper.GetCertificate(
                    reference.StoreName,
                    reference.StoreLocation,
                    reference.X509FindType,
                    reference.FindValue);
            }
            return null;
        }


    }
}
