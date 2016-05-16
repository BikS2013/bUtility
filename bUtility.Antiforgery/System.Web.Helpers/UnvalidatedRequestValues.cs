using System;
using System.Collections.Specialized;

namespace System.Web.Helpers
{
	/// <summary>Provides access to unvalidated form values in the <see cref="T:System.Web.HttpRequest" /> object.</summary>
	[Obsolete("Use System.Web.HttpRequest.Unvalidated instead.")]
	public sealed class UnvalidatedRequestValues
	{
		private readonly HttpRequestBase _request;

		/// <summary>Gets a collection of unvalidated form values that were posted from the browser.</summary>
		/// <returns>An unvalidated collection of form values.</returns>
		public NameValueCollection Form
		{
			get
			{
				return this._request.Unvalidated.Form;
			}
		}

		/// <summary>Gets a collection of unvalidated query-string values.</summary>
		/// <returns>A collection of unvalidated query-string values.</returns>
		public NameValueCollection QueryString
		{
			get
			{
				return this._request.Unvalidated.QueryString;
			}
		}

		/// <summary>Gets the specified unvalidated object from the collection of posted values in the <see cref="T:System.Web.HttpRequest" /> object.</summary>
		/// <returns>The specified member, or null if the specified item is not found.</returns>
		public string this[string key]
		{
			get
			{
				return this._request.Unvalidated[key];
			}
		}

		internal UnvalidatedRequestValues(HttpRequestBase request)
		{
			this._request = request;
		}
	}
}
