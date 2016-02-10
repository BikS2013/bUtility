using bUtility.Sts.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts
{
    public class SimpleStsConfiguration: SecurityTokenServiceConfiguration
    {
        static readonly object Locker = new object();
        private static readonly Dictionary<string, SimpleStsConfiguration> CurrentConfigurations = new Dictionary<string, SimpleStsConfiguration>();
        private static readonly Dictionary<string, Func<SecurityTokenHandler>> HandlerGenerator = new Dictionary<string, Func<SecurityTokenHandler>>
        {
            [TokenTypes.Saml11TokenProfile11] = ()=> new SamlSecurityTokenHandler() ,
            [TokenTypes.Saml2TokenProfile11] = () => new Saml2SecurityTokenHandler(),
            [TokenTypes.JsonWebToken] = () => new JwtSecurityTokenHandler()
        };


        public RelyingParty RelyingParty { get; private set;}

        public SimpleStsConfiguration(RelyingParty rp)
            : base(rp.IssuerName)
        {
            RelyingParty = rp;
            this.SecurityTokenService = typeof(SimpleSts);

            this.SecurityTokenHandlers.Clear();
            this.SecurityTokenHandlers.Add(HandlerGenerator[rp.TokenType]());

            if (rp.EncryptingCredentials != null)
            {
                ServiceCertificate = rp.EncryptingCredentials.Certificate;
                this.SecurityTokenHandlers.Add(new EncryptedSecurityTokenHandler());
            }
        }


        public static SimpleStsConfiguration ForRelyingParty(RelyingParty rp)
        {
            if (!CurrentConfigurations.ContainsKey(rp.Name))
            {
                lock (Locker)
                {
                    if (!CurrentConfigurations.ContainsKey(rp.Name))
                    {
                        CurrentConfigurations.Add(rp.Name, new SimpleStsConfiguration(rp));
                    }
                }
            }
            return CurrentConfigurations[rp.Name];
        }
    }
}
