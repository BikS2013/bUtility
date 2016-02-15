using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts
{
    public class SecurityTokenResolver : X509CertificateStoreTokenResolver
    {
        public SecurityTokenResolver()
            : base("My", StoreLocation.LocalMachine)
        {
        }
    }
}
