using bUtility.Sts.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IdentityModel;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Services;
using System.IdentityModel.Services.Configuration;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace bUtility.Sts
{
    public class SimpleSts: SecurityTokenService
    {
        public SimpleSts(SimpleStsConfiguration configuration) 
            : base(configuration)
        {
            // Ignore certificate errors as we will use our selfsigned ones and it will take too long to validate 
            // the certificate path.
            SecurityTokenServiceConfiguration.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
            SecurityTokenServiceConfiguration.CertificateValidator = new IgnoreCertificateErrorsValidator();

            SecurityTokenServiceConfiguration.DefaultTokenType = configuration.RelyingParty.TokenType; 
        }

        protected override Scope GetScope(ClaimsPrincipal principal, RequestSecurityToken request)
        {
            if (request.AppliesTo == null)
            {
                throw new InvalidRequestException($"token request from {principal?.Identity?.Name} - but no realm specified.");
            }

            var conf = (SimpleStsConfiguration)SecurityTokenServiceConfiguration;
            var rp = conf.RelyingParty;
            if (rp == null || rp.Realm != request.AppliesTo.Uri.ToString())
            {
                throw new InvalidRequestException(string.Format($"The AppliesTo uri {request.AppliesTo.Uri} is not registered as a relying party."));
            }

            var scope = new RequestScope(request.AppliesTo.Uri, rp);

            scope.ReplyToAddress = rp.RedirectUrl;

            request.TokenType = rp.TokenType;

            return scope;
        }

        protected override Lifetime GetTokenLifetime(Lifetime requestLifetime)
        {
            var scope = Scope as RequestScope;
            if (scope == null) throw new ApplicationException("No STS request scope found!");
            if (scope.RelyingParty.TokenLifeTime > 0)
            {
                return new Lifetime(DateTime.UtcNow, DateTime.UtcNow.AddMinutes(scope.RelyingParty.TokenLifeTime));
            }
            else
            {
                return base.GetTokenLifetime(requestLifetime);
            }
        }

        protected override ClaimsIdentity GetOutputClaimsIdentity(ClaimsPrincipal principal, RequestSecurityToken request, Scope scope)
        {

            if (principal == null)
                throw new SecurityException("Empty principal while creating claims identity!");

            ClaimsIdentity userIdentity = principal.Identities.First();
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
