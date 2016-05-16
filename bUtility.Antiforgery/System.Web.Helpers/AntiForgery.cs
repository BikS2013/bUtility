using System;
using System.ComponentModel;
using System.Web.Helpers.AntiXsrf;
using System.Web.Helpers.Claims;
using System.Web.Mvc;
using System.Web.WebPages.Resources;

namespace System.Web.Helpers
{
	/// <summary>Helps prevent malicious scripts from submitting forged page requests.</summary>
	public static class AntiForgery
	{
		private static readonly AntiForgeryWorker _worker = AntiForgery.CreateSingletonAntiForgeryWorker();

		private static AntiForgeryWorker CreateSingletonAntiForgeryWorker()
		{
			IAntiForgeryConfig config = new AntiForgeryConfigWrapper();
			IAntiForgeryTokenSerializer serializer = new AntiForgeryTokenSerializer(MachineKey45CryptoSystem.Instance);
			ITokenStore tokenStore = new AntiForgeryTokenStore(config, serializer);
			IClaimUidExtractor claimUidExtractor = new ClaimUidExtractor(config, ClaimsIdentityConverter.Default);
			ITokenValidator validator = new TokenValidator(config, claimUidExtractor);
			return new AntiForgeryWorker(serializer, config, tokenStore, validator);
		}

		/// <summary>Adds an authenticating token to a form to help protect against request forgery.</summary>
		/// <returns>Returns a string that contains the encrypted token value in a hidden HTML field.</returns>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Web.HttpContext" /> object is null.</exception>
		public static HtmlString GetHtml()
		{
			if (HttpContext.Current == null)
			{
				throw new ArgumentException(WebPageResources.HttpContextUnavailable);
			}
			TagBuilder formInputElement = AntiForgery._worker.GetFormInputElement(new HttpContextWrapper(HttpContext.Current));
			return formInputElement.ToHtmlString(TagRenderMode.SelfClosing);
		}

		/// <summary>Gets the search tokens.</summary>
		/// <param name="oldCookieToken">The previous cookie token.</param>
		/// <param name="newCookieToken">The new cookie token.</param>
		/// <param name="formToken">The form of the token.</param>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void GetTokens(string oldCookieToken, out string newCookieToken, out string formToken)
		{
			if (HttpContext.Current == null)
			{
				throw new ArgumentException(WebPageResources.HttpContextUnavailable);
			}
			AntiForgery._worker.GetTokens(new HttpContextWrapper(HttpContext.Current), oldCookieToken, out newCookieToken, out formToken);
		}

		/// <summary>Adds an authenticating token to a form to help protect against request forgery and lets callers specify authentication details.</summary>
		/// <returns>Returns the encrypted token value in a hidden HTML field.</returns>
		/// <param name="httpContext">The HTTP context data for a request.</param>
		/// <param name="salt">An optional string of random characters (such as Z*7g1&amp;p4) that is used to add complexity to the encryption for extra safety. The default is null.</param>
		/// <param name="domain">The domain of a web application that a request is submitted from.</param>
		/// <param name="path">The virtual root path of a web application that a request is submitted from.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="httpContext" /> is null.</exception>
		[EditorBrowsable(EditorBrowsableState.Never), Obsolete("This method is deprecated. Use the GetHtml() method instead. To specify a custom domain for the generated cookie, use the <httpCookies> configuration element. To specify custom data to be embedded within the token, use the static AntiForgeryConfig.AdditionalDataProvider property.", true)]
		public static HtmlString GetHtml(HttpContextBase httpContext, string salt, string domain, string path)
		{
			if (httpContext == null)
			{
				throw new ArgumentNullException("httpContext");
			}
			if (!string.IsNullOrEmpty(salt) || !string.IsNullOrEmpty(domain) || !string.IsNullOrEmpty(path))
			{
				throw new NotSupportedException("This method is deprecated. Use the GetHtml() method instead. To specify a custom domain for the generated cookie, use the <httpCookies> configuration element. To specify custom data to be embedded within the token, use the static AntiForgeryConfig.AdditionalDataProvider property.");
			}
			TagBuilder formInputElement = AntiForgery._worker.GetFormInputElement(httpContext);
			return formInputElement.ToHtmlString(TagRenderMode.SelfClosing);
		}

		/// <summary>Validates that input data from an HTML form field comes from the user who submitted the data.</summary>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Web.HttpContext" /> value is null.</exception>
		/// <exception cref="T:System.Web.Helpers.HttpAntiForgeryException">The HTTP cookie token that accompanies a valid request is missing-or-The form token is missing.-or-The form token value does not match the cookie token value.-or-The form token value does not match the cookie token value.</exception>
		public static void Validate()
		{
			if (HttpContext.Current == null)
			{
				throw new ArgumentException(WebPageResources.HttpContextUnavailable);
			}
			AntiForgery._worker.Validate(new HttpContextWrapper(HttpContext.Current));
		}

		/// <summary>Validates that input data from an HTML form field comes from the user who submitted the data.</summary>
		/// <param name="cookieToken">The cookie token value.</param>
		/// <param name="formToken">The token form.</param>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void Validate(string cookieToken, string formToken)
		{
			if (HttpContext.Current == null)
			{
				throw new ArgumentException(WebPageResources.HttpContextUnavailable);
			}
			AntiForgery._worker.Validate(new HttpContextWrapper(HttpContext.Current), cookieToken, formToken);
		}

		/// <summary>Validates that input data from an HTML form field comes from the user who submitted the data and lets callers specify additional validation details.</summary>
		/// <param name="httpContext">The HTTP context data for a request.</param>
		/// <param name="salt">An optional string of random characters (such as Z*7g1&amp;p4) that is used to decrypt an authentication token created by the <see cref="T:System.Web.Helpers.AntiForgery" /> class. The default is null.</param>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Web.HttpContext" /> value is null.</exception>
		/// <exception cref="T:System.Web.Helpers.HttpAntiForgeryException">The HTTP cookie token that accompanies a valid request is missing.-or-The form token is missing.-or-The form token value does not match the cookie token value.-or-The form token value does not match the cookie token value.-or-The <paramref name="salt" /> value supplied does not match the <paramref name="salt" /> value that was used to create the form token.</exception>
		[EditorBrowsable(EditorBrowsableState.Never), Obsolete("This method is deprecated. Use the Validate() method instead.", true)]
		public static void Validate(HttpContextBase httpContext, string salt)
		{
			if (httpContext == null)
			{
				throw new ArgumentNullException("httpContext");
			}
			if (!string.IsNullOrEmpty(salt))
			{
				throw new NotSupportedException("This method is deprecated. Use the Validate() method instead.");
			}
			AntiForgery._worker.Validate(httpContext);
		}
	}
}
