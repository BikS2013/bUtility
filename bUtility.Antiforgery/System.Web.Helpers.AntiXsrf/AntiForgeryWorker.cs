using System;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.WebPages.Resources;

namespace System.Web.Helpers.AntiXsrf
{
	internal sealed class AntiForgeryWorker
	{
		private readonly IAntiForgeryConfig _config;

		private readonly IAntiForgeryTokenSerializer _serializer;

		private readonly ITokenStore _tokenStore;

		private readonly ITokenValidator _validator;

		internal AntiForgeryWorker(IAntiForgeryTokenSerializer serializer, IAntiForgeryConfig config, ITokenStore tokenStore, ITokenValidator validator)
		{
			this._serializer = serializer;
			this._config = config;
			this._tokenStore = tokenStore;
			this._validator = validator;
		}

		private void CheckSSLConfig(HttpContextBase httpContext)
		{
			if (this._config.RequireSSL && !httpContext.Request.IsSecureConnection)
			{
				throw new InvalidOperationException(WebPageResources.AntiForgeryWorker_RequireSSL);
			}
		}

		private AntiForgeryToken DeserializeToken(string serializedToken)
		{
			if (string.IsNullOrEmpty(serializedToken))
			{
				return null;
			}
			return this._serializer.Deserialize(serializedToken);
		}

		private AntiForgeryToken DeserializeTokenNoThrow(string serializedToken)
		{
			AntiForgeryToken result;
			try
			{
				result = this.DeserializeToken(serializedToken);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		private static IIdentity ExtractIdentity(HttpContextBase httpContext)
		{
			if (httpContext != null)
			{
				IPrincipal user = httpContext.User;
				if (user != null)
				{
					return user.Identity;
				}
			}
			return null;
		}

		private AntiForgeryToken GetCookieTokenNoThrow(HttpContextBase httpContext)
		{
			AntiForgeryToken result;
			try
			{
				result = this._tokenStore.GetCookieToken(httpContext);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		public TagBuilder GetFormInputElement(HttpContextBase httpContext)
		{
			this.CheckSSLConfig(httpContext);
			AntiForgeryToken cookieTokenNoThrow = this.GetCookieTokenNoThrow(httpContext);
			AntiForgeryToken antiForgeryToken;
			AntiForgeryToken token;
			this.GetTokens(httpContext, cookieTokenNoThrow, out antiForgeryToken, out token);
			if (antiForgeryToken != null)
			{
				this._tokenStore.SaveCookieToken(httpContext, antiForgeryToken);
			}
			if (!this._config.SuppressXFrameOptionsHeader)
			{
				httpContext.Response.AddHeader("X-Frame-Options", "SAMEORIGIN");
			}
			TagBuilder tagBuilder = new TagBuilder("input");
			tagBuilder.Attributes["type"] = "hidden";
			tagBuilder.Attributes["name"] = this._config.FormFieldName;
			tagBuilder.Attributes["value"] = this._serializer.Serialize(token);
			return tagBuilder;
		}

		public void GetTokens(HttpContextBase httpContext, string serializedOldCookieToken, out string serializedNewCookieToken, out string serializedFormToken)
		{
			this.CheckSSLConfig(httpContext);
			AntiForgeryToken oldCookieToken = this.DeserializeTokenNoThrow(serializedOldCookieToken);
			AntiForgeryToken token;
			AntiForgeryToken token2;
			this.GetTokens(httpContext, oldCookieToken, out token, out token2);
			serializedNewCookieToken = this.Serialize(token);
			serializedFormToken = this.Serialize(token2);
		}

		private void GetTokens(HttpContextBase httpContext, AntiForgeryToken oldCookieToken, out AntiForgeryToken newCookieToken, out AntiForgeryToken formToken)
		{
			newCookieToken = null;
			if (!this._validator.IsCookieTokenValid(oldCookieToken))
			{
				AntiForgeryToken antiForgeryToken;
				newCookieToken = (antiForgeryToken = this._validator.GenerateCookieToken());
				oldCookieToken = antiForgeryToken;
			}
			formToken = this._validator.GenerateFormToken(httpContext, AntiForgeryWorker.ExtractIdentity(httpContext), oldCookieToken);
		}

		private string Serialize(AntiForgeryToken token)
		{
			if (token == null)
			{
				return null;
			}
			return this._serializer.Serialize(token);
		}

		public void Validate(HttpContextBase httpContext)
		{
			this.CheckSSLConfig(httpContext);
			AntiForgeryToken cookieToken = this._tokenStore.GetCookieToken(httpContext);
			AntiForgeryToken formToken = this._tokenStore.GetFormToken(httpContext);
			this._validator.ValidateTokens(httpContext, AntiForgeryWorker.ExtractIdentity(httpContext), cookieToken, formToken);
		}

		public void Validate(HttpContextBase httpContext, string cookieToken, string formToken)
		{
			this.CheckSSLConfig(httpContext);
			AntiForgeryToken cookieToken2 = this.DeserializeToken(cookieToken);
			AntiForgeryToken formToken2 = this.DeserializeToken(formToken);
			this._validator.ValidateTokens(httpContext, AntiForgeryWorker.ExtractIdentity(httpContext), cookieToken2, formToken2);
		}
	}
}
