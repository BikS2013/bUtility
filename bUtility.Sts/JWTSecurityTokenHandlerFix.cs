using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts
{
    public class JWTSecurityTokenHandlerFix: JwtSecurityTokenHandler
    {
        //Λύνει το πρόβλημα που περιγράφεται στο:
        //https://social.msdn.microsoft.com/Forums/vstudio/en-US/b399d302-753b-4357-a6ea-1c655a235e8c/jwtsecuritytokenhandler-notsupportedexception-idx11005?forum=Geneva
        public override SecurityKeyIdentifierClause CreateSecurityTokenReference(SecurityToken token, bool attached)
        {
            return null;
        }



        //Λύνει το πρόβλημα που περιγράφεται στο:
        //https://erikvanderstarre.wordpress.com/2014/12/14/using-the-jwtsecuritytokenhandler/
        /// <summary>
        /// Reads and validates a token encoded in JSON Compact serialized format.
        /// </summary>
        /// <param name="securityToken">A 'JSON Web Token' (JWT) that has been encoded as a JSON object. May be signed using 'JSON Web Signature' (JWS).</param>
        /// <returns>A <see cref="ReadOnlyCollection<ClaimsIdentity>"/> from the jwt.</returns>
        public override ReadOnlyCollection<ClaimsIdentity> ValidateToken(SecurityToken token)
        {
            JwtSecurityToken jwtToken = (JwtSecurityToken)token;

            // Get the configuration from the configuration file (element "issuerNameRegistry").
            ValidatingIssuerNameRegistry issuerNameRegistry = (ValidatingIssuerNameRegistry)
                      Configuration.IssuerNameRegistry;
            IssuingAuthority issuingAuthority = issuerNameRegistry.IssuingAuthorities.First();

            // Set the validation parameters from the configuration.
            var validationParameters = new TokenValidationParameters
            {
                // Get the audiences that are expected.
                ValidAudiences = Configuration.AudienceRestriction.AllowedAudienceUris.Select(s => s.ToString()),

                // Get the issuer that are expected.
                ValidIssuers = issuingAuthority.Issuers,

                // Get the certificate to validate signing from the certificate store (if configured).
                IssuerSigningKey = getCertificate(issuingAuthority.Thumbprints.FirstOrDefault()),

                // Get the symmetric key token that is used to sign (if configured).
                // Did not get this one working though.
                //IssuerSigningToken = GetSymmetricKeyToken(issuingAuthority.SymmetricKeys.FirstOrDefault()),

                // Get how to validate the certificate.
                CertificateValidator = Configuration.CertificateValidator,

                // Get if the token should be preserved.
                SaveSigninToken = Configuration.SaveBootstrapContext
            };


            // Call the correct validation method.
            SecurityToken validatedToken;
            ClaimsPrincipal validated = ValidateToken(jwtToken.RawData, validationParameters, out validatedToken);

            // Return the claim identities.
            return new ReadOnlyCollection<ClaimsIdentity>(validated.Identities.ToList());
        }


        static ConcurrentDictionary<string, X509Certificate2> IssuerKeys = new ConcurrentDictionary<string, X509Certificate2>();
        public static void AddIssuerKey(X509Certificate2 key)
        {
            lock (IssuerKeys)
            {
                if (!IssuerKeys.ContainsKey(key.Thumbprint.ToUpper()))
                {
                    IssuerKeys[key.Thumbprint] = key;
                }
            }
        }

        SecurityKey getCertificate( string thumbprint)
        {
            if ( !IssuerKeys.ContainsKey(thumbprint.ToUpper()))
            {
                return null; 
            }
            return new X509AsymmetricSecurityKey(IssuerKeys[thumbprint.ToUpper()]);
        }

    }
}
