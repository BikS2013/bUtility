using System;
using System.Security.Principal;

namespace System.Web.Helpers.AntiXsrf
{
	internal interface ITokenValidator
	{
		AntiForgeryToken GenerateCookieToken();

		AntiForgeryToken GenerateFormToken(HttpContextBase httpContext, IIdentity identity, AntiForgeryToken cookieToken);

		bool IsCookieTokenValid(AntiForgeryToken cookieToken);

		void ValidateTokens(HttpContextBase httpContext, IIdentity identity, AntiForgeryToken cookieToken, AntiForgeryToken formToken);
	}
}
