using System;
using System.Security.Principal;

namespace System.Web.Helpers.AntiXsrf
{
	internal interface IClaimUidExtractor
	{
		BinaryBlob ExtractClaimUid(IIdentity identity);
	}
}
