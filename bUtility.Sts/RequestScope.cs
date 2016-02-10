using bUtility.Sts.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts
{
    public class RequestScope: Scope
    {
        public RelyingParty RelyingParty { get; private set; }

        public RequestScope(Uri uri, RelyingParty rp):
            base(uri.ToString(),rp.SigningCredentials)
        {
            RelyingParty = rp;
            if (rp.EncryptingCredentials != null)
            {
                EncryptingCredentials = rp.EncryptingCredentials;
                TokenEncryptionRequired = true;
                SymmetricKeyEncryptionRequired = true;
            }
            else
            {
                TokenEncryptionRequired = false;
                SymmetricKeyEncryptionRequired = false;
            }
        }

    }
}
