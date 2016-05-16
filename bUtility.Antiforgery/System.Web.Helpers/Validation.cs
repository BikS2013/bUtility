using System;

namespace System.Web.Helpers
{
	/// <summary>Excludes fields of the Request object from being checked for potentially unsafe HTML markup and client script.</summary>
	[Obsolete("Use System.Web.HttpRequest.Unvalidated instead.")]
	public static class Validation
	{
		/// <summary>Returns all values from the Request object (including form fields, cookies, and the query string) without checking them first for HTML markup and client script.</summary>
		/// <returns>An object that contains unvalidated versions of the form, cookie, and query-string values.</returns>
		/// <param name="request">The <see cref="T:System.Web.HttpRequest" /> object that contains values to exclude from validation.</param>
		public static UnvalidatedRequestValues Unvalidated(this HttpRequestBase request)
		{
			return Validation.Unvalidated((HttpRequest)null);
		}

		/// <summary>Returns a version of form values, cookies, and query-string variables without checking them first for HTML markup and client script.</summary>
		/// <returns>An object that contains unvalidated versions of the form and query-string values.</returns>
		/// <param name="request">The <see cref="T:System.Web.HttpRequest" /> object that contains values to exclude from request validation.</param>
		public static UnvalidatedRequestValues Unvalidated(this HttpRequest request)
		{
			HttpContext current = HttpContext.Current;
			return new UnvalidatedRequestValues(new HttpRequestWrapper(current.Request));
		}

		/// <summary>Returns the specified value from the Request object without checking it first for HTML markup and client script.</summary>
		/// <returns>A string that contains unvalidated text from the specified field, cookie, or query-string value.</returns>
		/// <param name="request">The <see cref="T:System.Web.HttpRequestBase" /> object that contains values to exclude from validation.</param>
		/// <param name="key">The name of the field to exclude from validation. <paramref name="key" /> can refer to a form field, to a cookie, or to the query-string variable.</param>
		public static string Unvalidated(this HttpRequestBase request, string key)
		{
			return request.Unvalidated()[key];
		}

		/// <summary>Returns a value from the specified form field, cookie, or query-string variable without checking it first for HTML markup and client script.</summary>
		/// <returns>A string that contains unvalidated text from the specified field, cookie, or query-string value.</returns>
		/// <param name="request">The <see cref="T:System.Web.HttpRequest" /> object that contains values to exclude from validation.</param>
		/// <param name="key">The name of the field to exclude from validation. <paramref name="key" /> can refer to a form field, to a cookie, or to the query-string variable.</param>
		public static string Unvalidated(this HttpRequest request, string key)
		{
			return request.Unvalidated()[key];
		}
	}
}
