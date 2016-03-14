using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts.Configuration
{
    public interface IRelyingParty
    {
        string Name { get; }
        long TokenLifeTime { get; }
        string RedirectUrl { get; }
        string Realm { get; }
        string AuthenticationUrl { get; }
        string IssuerName { get; }
        string TokenType { get; }

        X509Certificate2 GetEncryptingCertificate();
        X509Certificate2 GetSigningCertificate();
    }
}
