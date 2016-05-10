using System;

namespace System.Web.Helpers.AntiXsrf
{
	internal interface ITokenStore
	{
		AntiForgeryToken GetCookieToken(HttpContextBase httpContext);

		AntiForgeryToken GetFormToken(HttpContextBase httpContext);

		void SaveCookieToken(HttpContextBase httpContext, AntiForgeryToken token);
	}
}
