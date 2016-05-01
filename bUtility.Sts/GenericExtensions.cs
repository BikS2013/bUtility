using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace bUtility.Sts
{
    public static class GenericExtensions
    {
        public static ClaimsPrincipal GetPrincipal(this string token)
        {
            var handlers = FederatedAuthentication.FederationConfiguration.IdentityConfiguration.SecurityTokenHandlers;


            using (var sr = new StringReader(token))
            {
                using (var xmlReader = new XmlTextReader(sr))
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
            return null;
        }

        public static string GetJwtToken(this ClaimsIdentity identity, SecurityTokenDescriptor tokenDescriptor)
        {
            if (identity == null || tokenDescriptor == null) return null;
            tokenDescriptor.Subject = identity;
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
        public static ClaimsPrincipal ReadJwtToken(this string token, SecurityTokenDescriptor tokenDescriptor )
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var certificate = (tokenDescriptor.SigningCredentials as X509SigningCredentials).Certificate;
            var validationParameters = new TokenValidationParameters
            {
                ValidAudience = tokenDescriptor.AppliesToAddress, 
                ValidIssuer = tokenDescriptor.TokenIssuerName,
                IssuerSigningToken = new X509SecurityToken(certificate)
            };

            var validatedToken = new JwtSecurityToken() as SecurityToken;
            var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

            return principal;
        }
    }
}
