using bUtility.Sts.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace bUtility.Sts
{
    public static class RelyingPartyExtensions
    {
        public static SignInRequestMessage GetSignInRequestMessage(this RelyingParty rp, Uri baseUri)
        {
            return new SignInRequestMessage(baseUri, rp.Realm);
        }
        public static SimpleStsConfiguration GetStsConfiguration(this RelyingParty rp)
        {
            return SimpleStsConfiguration.ForRelyingParty(rp);
        }

        public static SignInResponseMessage ProcessSignInRequest(this RelyingParty rp, Uri baseUri, ClaimsPrincipal principal)
        {
            var sts = new SimpleSts(rp.GetStsConfiguration());
            return FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(
                rp.GetSignInRequestMessage(baseUri), principal, sts);
        }

        public static void HandleSignIn(this HttpResponse httpResponse, Uri baseUri, RelyingParty rp, ClaimsPrincipal principal)
        {
            var resp = rp.ProcessSignInRequest(baseUri, principal);

            var httpMessage = resp.WriteFormPost()
                .Replace("window.setTimeout('document.forms[0].submit()', 0);",
                    "window.setTimeout(submit, 0);function submit(){document.forms[0].submit();}");

            httpResponse.Write(httpMessage);
        }
    }
}
