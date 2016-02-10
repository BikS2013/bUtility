using bUtility.Sts.local;
using System;
using System.Collections.Generic;
using System.IdentityModel;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts
{
    public class SimpleSts: SecurityTokenService
    {
        public SimpleSts(SimpleStsConfiguration securityTokenServiceConfiguration) 
            : base(securityTokenServiceConfiguration)
        {
            // Ignore certificate errors as we will use our selfsigned ones and it will take too long to validate 
            // the certificate path.
            this.SecurityTokenServiceConfiguration.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
            this.SecurityTokenServiceConfiguration.CertificateValidator = new CertificateHelper.IgnoreCertificateErrorsValidator();

            this.SecurityTokenServiceConfiguration.DefaultTokenType = securityTokenServiceConfiguration.RelyingParty.TokenType; 
        }

        protected override Scope GetScope(ClaimsPrincipal principal, RequestSecurityToken rst)
        {
            if (rst.AppliesTo == null)
            {
                throw new InvalidRequestException($"token request from {principal.Identity.Name} - but no realm specified.");
            }

            var authenticationMethod = principal.Identities.First().FindFirst(ClaimTypes.AuthenticationMethod);
            if (authenticationMethod == null)
            {
                throw new ApplicationException("Authentication method not defined!");
            }

            var conf = (SimpleStsConfiguration)this.SecurityTokenServiceConfiguration;
            var rp = conf.RelyingParty;
            if (rp == null || rp.Realm != rst.AppliesTo.Uri.ToString())
            {
                throw new InvalidRequestException(string.Format($"The AppliesTo uri {rst.AppliesTo.Uri} is not registered as a relying party."));
            }

            var scope = new RequestScope(rst.AppliesTo.Uri, rp);

            scope.ReplyToAddress = rp.RedirectUrl;

            rst.TokenType = rp.TokenType;

            return scope;
        }

        protected override ClaimsIdentity GetOutputClaimsIdentity(ClaimsPrincipal principal, RequestSecurityToken request, Scope scope)
        {

            if (principal == null)
                throw new SecurityException("Empty principal while creating claims identity!");

            ClaimsIdentity userIdentity = principal.Identities.First();
            if (userIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier) == null)
            {
                throw new SecurityException($"No user name has been specified! Original identity: {userIdentity.ToJson()}");
            }
            var inheritedClaims =
                userIdentity.Claims.Where(i => !i.Type.In(ClaimTypes.AuthenticationInstant, ClaimTypes.AuthenticationMethod))
                .Select(c => new Claim(c.Type, c.Value));


            var relyingParty = (scope as RequestScope).RelyingParty;

            var outputIdentity = new ClaimsIdentity(relyingParty.IssuerName);
            // We also have the ClaimTypes.AuthenticationMethod that shows which was the original authenticator
            outputIdentity.AddClaim(new Claim(ClaimTypes.AuthenticationInstant, DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"), ClaimValueTypes.DateTime, relyingParty.IssuerName));
            outputIdentity.AddClaim(new Claim(ClaimTypes.AuthenticationMethod, relyingParty.AuthenticationUrl, ClaimValueTypes.String, relyingParty.IssuerName));

            outputIdentity.AddClaims(inheritedClaims);

            return outputIdentity;
        }


    }
}
