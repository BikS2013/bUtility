using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Web.WebPages.Resources;

namespace System.Web.Mvc
{
	/// <summary>This type/member supports the .NET Framework infrastructure and is not intended to be used directly from your code.</summary>
	[TypeForwardedFrom("System.Web.Mvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
	[Serializable]
	public sealed class HttpAntiForgeryException : HttpException
	{
		/// <summary>This member supports the .NET Framework infrastructure and is not intended to be used directly from your code.Initializes a new instance of the <see cref="T:System.Web.Mvc.HttpAntiForgeryException" /> class.</summary>
		public HttpAntiForgeryException()
		{
		}

		private HttpAntiForgeryException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>This member supports the .NET Framework infrastructure and is not intended to be used directly from your code. Initializes a new instance of the <see cref="T:System.Web.Mvc.HttpAntiForgeryException" /> class.</summary>
		/// <param name="message">The containing message.</param>
		public HttpAntiForgeryException(string message) : base(message)
		{
		}

		private HttpAntiForgeryException(string message, params object[] args) : this(string.Format(CultureInfo.CurrentCulture, message, args))
		{
		}

		/// <summary>This member supports the .NET Framework infrastructure and is not intended to be used directly from your code. Initializes a new instance of the <see cref="T:System.Web.Mvc.HttpAntiForgeryException" /> class.</summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		public HttpAntiForgeryException(string message, Exception innerException) : base(message, innerException)
		{
		}

		internal static HttpAntiForgeryException CreateAdditionalDataCheckFailedException()
		{
			return new HttpAntiForgeryException(WebPageResources.AntiForgeryToken_AdditionalDataCheckFailed);
		}

		internal static HttpAntiForgeryException CreateClaimUidMismatchException()
		{
			return new HttpAntiForgeryException(WebPageResources.AntiForgeryToken_ClaimUidMismatch);
		}

		internal static HttpAntiForgeryException CreateCookieMissingException(string cookieName)
		{
			return new HttpAntiForgeryException(WebPageResources.AntiForgeryToken_CookieMissing, new object[]
			{
				cookieName
			});
		}

		internal static HttpAntiForgeryException CreateDeserializationFailedException()
		{
			return new HttpAntiForgeryException(WebPageResources.AntiForgeryToken_DeserializationFailed);
		}

		internal static HttpAntiForgeryException CreateFormFieldMissingException(string formFieldName)
		{
			return new HttpAntiForgeryException(WebPageResources.AntiForgeryToken_FormFieldMissing, new object[]
			{
				formFieldName
			});
		}

		internal static HttpAntiForgeryException CreateSecurityTokenMismatchException()
		{
			return new HttpAntiForgeryException(WebPageResources.AntiForgeryToken_SecurityTokenMismatch);
		}

		internal static HttpAntiForgeryException CreateTokensSwappedException(string cookieName, string formFieldName)
		{
			return new HttpAntiForgeryException(WebPageResources.AntiForgeryToken_TokensSwapped, new object[]
			{
				cookieName,
				formFieldName
			});
		}

		internal static HttpAntiForgeryException CreateUsernameMismatchException(string usernameInToken, string currentUsername)
		{
			return new HttpAntiForgeryException(WebPageResources.AntiForgeryToken_UsernameMismatch, new object[]
			{
				usernameInToken,
				currentUsername
			});
		}
	}
}
