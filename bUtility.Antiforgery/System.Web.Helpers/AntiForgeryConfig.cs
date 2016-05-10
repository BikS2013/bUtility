using System;
using System.ComponentModel;
using System.Text;

namespace System.Web.Helpers
{
	/// <summary>Provides programmatic configuration for the anti-forgery token system.</summary>
	public static class AntiForgeryConfig
	{
		internal const string AntiForgeryTokenFieldName = "__RequestVerificationToken";

		private static string _cookieName;

		private static string _uniqueClaimTypeIdentifier;

		/// <summary>Gets a data provider that can provide additional data to put into all generated tokens and that can validate additional data in incoming tokens.</summary>
		/// <returns>The data provider.</returns>
		public static IAntiForgeryAdditionalDataProvider AdditionalDataProvider
		{
			get;
			set;
		}

		/// <summary>Gets or sets the name of the cookie that is used by the anti-forgery system.</summary>
		/// <returns>The cookie name.</returns>
		public static string CookieName
		{
			get
			{
				if (AntiForgeryConfig._cookieName == null)
				{
					AntiForgeryConfig._cookieName = AntiForgeryConfig.GetAntiForgeryCookieName();
				}
				return AntiForgeryConfig._cookieName;
			}
			set
			{
				AntiForgeryConfig._cookieName = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the anti-forgery cookie requires SSL in order to be returned to the server.</summary>
		/// <returns>true if SSL is required to return the anti-forgery cookie to the server; otherwise, false. </returns>
		public static bool RequireSsl
		{
			get;
			set;
		}

		/// <summary>Specifies whether to suppress the generation of X-Frame-Options header which is used to prevent ClickJacking. By default, the X-Frame-Options header is generated with the value SAMEORIGIN. If this setting is 'true', the X-Frame-Options header will not be generated for the response.</summary>
		public static bool SuppressXFrameOptionsHeader
		{
			get;
			set;
		}

		/// <summary>Gets or sets a value that indicates whether the anti-forgery system should skip checking for conditions that might indicate misuse of the system.</summary>
		/// <returns>true if the anti-forgery system should not check for possible misuse; otherwise, false.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool SuppressIdentityHeuristicChecks
		{
			get;
			set;
		}

		/// <summary>If claims-based authorization is in use, gets or sets the claim type from the identity that is used to uniquely identify the user.</summary>
		/// <returns>The claim type.</returns>
		public static string UniqueClaimTypeIdentifier
		{
			get
			{
				return AntiForgeryConfig._uniqueClaimTypeIdentifier ?? string.Empty;
			}
			set
			{
				AntiForgeryConfig._uniqueClaimTypeIdentifier = value;
			}
		}

		private static string GetAntiForgeryCookieName()
		{
			return AntiForgeryConfig.GetAntiForgeryCookieName(HttpRuntime.AppDomainAppVirtualPath);
		}

		internal static string GetAntiForgeryCookieName(string appPath)
		{
			if (string.IsNullOrEmpty(appPath) || appPath == "/")
			{
				return "__RequestVerificationToken";
			}
			return "__RequestVerificationToken_" + HttpServerUtility.UrlTokenEncode(Encoding.UTF8.GetBytes(appPath));
		}
	}
}
