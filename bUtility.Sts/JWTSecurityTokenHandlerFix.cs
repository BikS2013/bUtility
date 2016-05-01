using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts
{
    //Λύνει το πρόβλημα που περιγράφεται στο:
    //https://social.msdn.microsoft.com/Forums/vstudio/en-US/b399d302-753b-4357-a6ea-1c655a235e8c/jwtsecuritytokenhandler-notsupportedexception-idx11005?forum=Geneva
    public class JWTSecurityTokenHandlerFix: JwtSecurityTokenHandler
    {
        public override SecurityKeyIdentifierClause CreateSecurityTokenReference(SecurityToken token, bool attached)
        {
            return null;
        }
    }
}
