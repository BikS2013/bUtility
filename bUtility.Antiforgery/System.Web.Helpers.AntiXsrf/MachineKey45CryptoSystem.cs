using System;
using System.Web.Security;

namespace System.Web.Helpers.AntiXsrf
{
	internal sealed class MachineKey45CryptoSystem : ICryptoSystem
	{
		private static readonly string[] _purposes = new string[]
		{
			"System.Web.Helpers.AntiXsrf.AntiForgeryToken.v1"
		};

		private static readonly MachineKey45CryptoSystem _singletonInstance = MachineKey45CryptoSystem.GetSingletonInstance();

		public static MachineKey45CryptoSystem Instance
		{
			get
			{
				return MachineKey45CryptoSystem._singletonInstance;
			}
		}

		private static MachineKey45CryptoSystem GetSingletonInstance()
		{
			return new MachineKey45CryptoSystem();
		}

		public string Protect(byte[] data)
		{
			byte[] input = MachineKey.Protect(data, MachineKey45CryptoSystem._purposes);
			return HttpServerUtility.UrlTokenEncode(input);
		}

		public byte[] Unprotect(string protectedData)
		{
			byte[] protectedData2 = HttpServerUtility.UrlTokenDecode(protectedData);
			return MachineKey.Unprotect(protectedData2, MachineKey45CryptoSystem._purposes);
		}
	}
}
