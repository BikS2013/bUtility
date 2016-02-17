using bUtility.Sts.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace bUtility.Sts
{
    public static class RelyingPartyExtensions
    {
        public static X509Certificate2 GetSigningCertificate(this IRelyingParty rp)
        {
            return rp.SigningCertificate.GetCertificate();
        }

        public static X509Certificate2 GetEncryptingCertificate(this IRelyingParty rp)
        {
            return rp.EncryptingCertificate.GetCertificate();
        }
        public static X509SigningCredentials GetSigningCredentials(this IRelyingParty rp)
        {
            return rp.SigningCertificate.GetSigningCredentials();
        }

        public static X509EncryptingCredentials GetEncryptingCredentials(this IRelyingParty rp)
        {
            return rp.EncryptingCertificate.GetEncryptingCredentials();
        }


        public static SignInRequestMessage GetSignInRequestMessage(this IRelyingParty rp, Uri baseUri)
        {
            return new SignInRequestMessage(baseUri, rp.Realm);
        }
        public static SimpleStsConfiguration GetStsConfiguration(this IRelyingParty rp)
        {
            return SimpleStsConfiguration.ForRelyingParty(rp);
        }

        public static SignInResponseMessage ProcessSignInRequest(this IRelyingParty rp, Uri baseUri, ClaimsPrincipal principal)
        {
            var sts = new SimpleSts(rp.GetStsConfiguration());
            return FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(
                rp.GetSignInRequestMessage(baseUri), principal, sts);
        }

        public static void HandleSignIn(this HttpResponse httpResponse, Uri baseUri, IRelyingParty rp, ClaimsPrincipal principal)
        {
            var resp = rp.ProcessSignInRequest(baseUri, principal);

            var httpMessage = resp.WriteFormPost()
                .Replace("window.setTimeout('document.forms[0].submit()', 0);",
                    "window.setTimeout(submit, 0);function submit(){document.forms[0].submit();}");

            httpResponse.Write(httpMessage);
        }
    }
}
