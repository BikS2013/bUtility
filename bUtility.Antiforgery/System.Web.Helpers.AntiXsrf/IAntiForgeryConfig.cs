using System;

namespace System.Web.Helpers.AntiXsrf
{
	internal interface IAntiForgeryConfig
	{
		IAntiForgeryAdditionalDataProvider AdditionalDataProvider
		{
			get;
		}

		string CookieName
		{
			get;
		}

		string FormFieldName
		{
			get;
		}

		bool RequireSSL
		{
			get;
		}

		bool SuppressIdentityHeuristicChecks
		{
			get;
		}

		string UniqueClaimTypeIdentifier
		{
			get;
		}

		bool SuppressXFrameOptionsHeader
		{
			get;
		}
	}
}
