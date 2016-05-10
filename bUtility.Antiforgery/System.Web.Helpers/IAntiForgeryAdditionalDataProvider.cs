using System;

namespace System.Web.Helpers
{
	/// <summary>Provides a way to include or validate custom data for anti-forgery tokens.</summary>
	public interface IAntiForgeryAdditionalDataProvider
	{
		/// <summary>Provides additional data to store for the anti-forgery tokens that are generated during this request.</summary>
		/// <returns>The supplemental data to embed in the anti-forgery token.</returns>
		/// <param name="context">Information about the current request.</param>
		string GetAdditionalData(HttpContextBase context);

		/// <summary>Validates additional data that was embedded inside an incoming anti-forgery token.</summary>
		/// <returns>true if the data is valid, or false if the data is invalid.</returns>
		/// <param name="context">Information about the current request.</param>
		/// <param name="additionalData">The supplemental data that was embedded in the token.</param>
		bool ValidateAdditionalData(HttpContextBase context, string additionalData);
	}
}
