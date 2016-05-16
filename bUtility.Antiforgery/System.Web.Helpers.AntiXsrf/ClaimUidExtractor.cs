using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web.Helpers.Claims;
using System.Web.WebPages.Resources;

namespace System.Web.Helpers.AntiXsrf
{
	internal sealed class ClaimUidExtractor : IClaimUidExtractor
	{
		internal const string NameIdentifierClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

		internal const string IdentityProviderClaimType = "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider";

		private readonly ClaimsIdentityConverter _claimsIdentityConverter;

		private readonly IAntiForgeryConfig _config;

		internal ClaimUidExtractor(IAntiForgeryConfig config, ClaimsIdentityConverter claimsIdentityConverter)
		{
			this._config = config;
			this._claimsIdentityConverter = claimsIdentityConverter;
		}

		public BinaryBlob ExtractClaimUid(IIdentity identity)
		{
			if (identity == null || !identity.IsAuthenticated || this._config.SuppressIdentityHeuristicChecks)
			{
				return null;
			}
			ClaimsIdentity claimsIdentity = this._claimsIdentityConverter.TryConvert(identity);
			if (claimsIdentity == null)
			{
				return null;
			}
			string[] uniqueIdentifierParameters = ClaimUidExtractor.GetUniqueIdentifierParameters(claimsIdentity, this._config.UniqueClaimTypeIdentifier);
			byte[] data = CryptoUtil.ComputeSHA256(uniqueIdentifierParameters);
			return new BinaryBlob(256, data);
		}

		internal static string[] GetUniqueIdentifierParameters(ClaimsIdentity claimsIdentity, string uniqueClaimTypeIdentifier)
		{
			IEnumerable<Claim> claims = claimsIdentity.GetClaims();
			if (!string.IsNullOrEmpty(uniqueClaimTypeIdentifier))
			{
				Claim claim4 = claims.SingleOrDefault((Claim claim) => string.Equals(uniqueClaimTypeIdentifier, claim.ClaimType, StringComparison.Ordinal));
				if (claim4 == null || string.IsNullOrEmpty(claim4.Value))
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, WebPageResources.ClaimUidExtractor_ClaimNotPresent, new object[]
					{
						uniqueClaimTypeIdentifier
					}));
				}
				return new string[]
				{
					uniqueClaimTypeIdentifier,
					claim4.Value
				};
			}
			else
			{
				Claim claim2 = claims.SingleOrDefault((Claim claim) => string.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", claim.ClaimType, StringComparison.Ordinal));
				Claim claim3 = claims.SingleOrDefault((Claim claim) => string.Equals("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", claim.ClaimType, StringComparison.Ordinal));
				if (claim2 == null || string.IsNullOrEmpty(claim2.Value) || claim3 == null || string.IsNullOrEmpty(claim3.Value))
				{
					throw new InvalidOperationException(WebPageResources.ClaimUidExtractor_DefaultClaimsNotPresent);
				}
				return new string[]
				{
					"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
					claim2.Value,
					"http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
					claim3.Value
				};
			}
		}
	}
}
