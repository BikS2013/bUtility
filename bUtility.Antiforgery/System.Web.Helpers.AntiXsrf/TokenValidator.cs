using System;
using System.Globalization;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.WebPages.Resources;

namespace System.Web.Helpers.AntiXsrf
{
	internal sealed class TokenValidator : ITokenValidator
	{
		private readonly IClaimUidExtractor _claimUidExtractor;

		private readonly IAntiForgeryConfig _config;

		internal TokenValidator(IAntiForgeryConfig config, IClaimUidExtractor claimUidExtractor)
		{
			this._config = config;
			this._claimUidExtractor = claimUidExtractor;
		}

		public AntiForgeryToken GenerateCookieToken()
		{
			return new AntiForgeryToken
			{
				IsSessionToken = true
			};
		}

		public AntiForgeryToken GenerateFormToken(HttpContextBase httpContext, IIdentity identity, AntiForgeryToken cookieToken)
		{
			AntiForgeryToken antiForgeryToken = new AntiForgeryToken
			{
				SecurityToken = cookieToken.SecurityToken,
				IsSessionToken = false
			};
			bool flag = false;
			if (identity != null && identity.IsAuthenticated)
			{
				if (!this._config.SuppressIdentityHeuristicChecks)
				{
					flag = true;
				}
				antiForgeryToken.ClaimUid = this._claimUidExtractor.ExtractClaimUid(identity);
				if (antiForgeryToken.ClaimUid == null)
				{
					antiForgeryToken.Username = identity.Name;
				}
			}
			if (this._config.AdditionalDataProvider != null)
			{
				antiForgeryToken.AdditionalData = this._config.AdditionalDataProvider.GetAdditionalData(httpContext);
			}
			if (flag && string.IsNullOrEmpty(antiForgeryToken.Username) && antiForgeryToken.ClaimUid == null && string.IsNullOrEmpty(antiForgeryToken.AdditionalData))
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, WebPageResources.TokenValidator_AuthenticatedUserWithoutUsername, new object[]
				{
					identity.GetType()
				}));
			}
			return antiForgeryToken;
		}

		public bool IsCookieTokenValid(AntiForgeryToken cookieToken)
		{
			return cookieToken != null && cookieToken.IsSessionToken;
		}

		public void ValidateTokens(HttpContextBase httpContext, IIdentity identity, AntiForgeryToken sessionToken, AntiForgeryToken fieldToken)
		{
			if (sessionToken == null)
			{
				throw HttpAntiForgeryException.CreateCookieMissingException(this._config.CookieName);
			}
			if (fieldToken == null)
			{
				throw HttpAntiForgeryException.CreateFormFieldMissingException(this._config.FormFieldName);
			}
			if (!sessionToken.IsSessionToken || fieldToken.IsSessionToken)
			{
				throw HttpAntiForgeryException.CreateTokensSwappedException(this._config.CookieName, this._config.FormFieldName);
			}
			if (!object.Equals(sessionToken.SecurityToken, fieldToken.SecurityToken))
			{
				throw HttpAntiForgeryException.CreateSecurityTokenMismatchException();
			}
			string text = string.Empty;
			BinaryBlob binaryBlob = null;
			if (identity != null && identity.IsAuthenticated)
			{
				binaryBlob = this._claimUidExtractor.ExtractClaimUid(identity);
				if (binaryBlob == null)
				{
					text = (identity.Name ?? string.Empty);
				}
			}
			bool flag = text.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || text.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
			if (!string.Equals(fieldToken.Username, text, flag ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
			{
				throw HttpAntiForgeryException.CreateUsernameMismatchException(fieldToken.Username, text);
			}
			if (!object.Equals(fieldToken.ClaimUid, binaryBlob))
			{
				throw HttpAntiForgeryException.CreateClaimUidMismatchException();
			}
			if (this._config.AdditionalDataProvider != null && !this._config.AdditionalDataProvider.ValidateAdditionalData(httpContext, fieldToken.AdditionalData))
			{
				throw HttpAntiForgeryException.CreateAdditionalDataCheckFailedException();
			}
		}
	}
}
