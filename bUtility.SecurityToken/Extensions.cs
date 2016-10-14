using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace bUtility.SecurityToken
{
    public static class Extensions
    {
        static ClaimsPrincipal Anonymous
        {
            get
            {
                var anonId = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "") });
                return new ClaimsPrincipal(anonId);
            }
        }
        public static ClaimsPrincipal GetClaimsPrincipal(this AuthenticationHeaderValue authenticationHeader, string expectedScheme)
        {
            if (authenticationHeader?.Scheme?.Equals(expectedScheme, StringComparison.InvariantCulture) == true)
            {
                return AuthenticateAuthorizationHeader(authenticationHeader.Parameter);
            }
            return Anonymous;
        }

        static ClaimsPrincipal AuthenticateAuthorizationHeader(string credential)
        {
            if (!string.IsNullOrEmpty(credential))
            {
                // Get the base64 encoded token (converted that way to make it http friendly)
                credential = Encoding.UTF8.GetString(Convert.FromBase64String(credential));
            }
            var handlers = FederatedAuthentication.FederationConfiguration.IdentityConfiguration.SecurityTokenHandlers;

            using (var textReader = new StringReader(credential))
            {
                using (var xmlReader = new XmlTextReader(textReader))
                {
                    var tokens = handlers.ReadToken(xmlReader);
                    if (tokens != null)
                    {
                        var identities = handlers.ValidateToken(tokens);
                        if (identities != null)
                        {
                            var principal = new ClaimsPrincipal(identities);
                            if (principal != null)
                            {
                                return principal;
                            }
                        }
                    }
                }
            }
            return Anonymous;
        }

        public static string GetBootstrapContextToken(this IIdentity identity)
        {
            if (identity as ClaimsIdentity != null)
            {
                var context = (identity as ClaimsIdentity).BootstrapContext as BootstrapContext;
                if (context != null)
                {
                    var token = context.SecurityToken;
                    if (context.Token != null) return context.Token;

                    StringBuilder output = new StringBuilder(128);
                    context.SecurityTokenHandler.WriteToken(new XmlTextWriter(new StringWriter(output)), token);
                    return output.ToString();
                }
            }
            return null;
        }

        public static AuthenticationHeaderValue GetAuthenticationHeader(this IIdentity identity, string scheme = "SAML")
        {
            var token = identity.GetBootstrapContextToken();
            if (token != null)
            {
                var httpFriendlyToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                return new AuthenticationHeaderValue(scheme, httpFriendlyToken);
            }
            return null;
        }
    }
}
