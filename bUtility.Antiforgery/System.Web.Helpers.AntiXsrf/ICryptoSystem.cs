using System;

namespace System.Web.Helpers.AntiXsrf
{
	internal interface ICryptoSystem
	{
		string Protect(byte[] data);

		byte[] Unprotect(string protectedData);
	}
}
