using System;

namespace System.Web.Helpers.AntiXsrf
{
	internal sealed class AntiForgeryTokenStore : ITokenStore
	{
		private readonly IAntiForgeryConfig _config;

		private readonly IAntiForgeryTokenSerializer _serializer;

		internal AntiForgeryTokenStore(IAntiForgeryConfig config, IAntiForgeryTokenSerializer serializer)
		{
			this._config = config;
			this._serializer = serializer;
		}

		public AntiForgeryToken GetCookieToken(HttpContextBase httpContext)
		{
			HttpCookie httpCookie = httpContext.Request.Cookies[this._config.CookieName];
			if (httpCookie == null || string.IsNullOrEmpty(httpCookie.Value))
			{
				return null;
			}
			return this._serializer.Deserialize(httpCookie.Value);
		}

		public AntiForgeryToken GetFormToken(HttpContextBase httpContext)
		{
			string text = httpContext.Request.Form[this._config.FormFieldName];
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			return this._serializer.Deserialize(text);
		}

		public void SaveCookieToken(HttpContextBase httpContext, AntiForgeryToken token)
		{
			string value = this._serializer.Serialize(token);
			HttpCookie httpCookie = new HttpCookie(this._config.CookieName, value)
			{
				HttpOnly = true
			};
			if (this._config.RequireSSL)
			{
				httpCookie.Secure = true;
			}
			httpContext.Response.Cookies.Set(httpCookie);
		}
	}
}
