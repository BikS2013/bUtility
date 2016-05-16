using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Principal;
using System.Web.Security;

namespace System.Web.Helpers.Claims
{
	internal sealed class ClaimsIdentityConverter
	{
		private static readonly MethodInfo _claimsIdentityTryConvertOpenMethod = typeof(ClaimsIdentity).GetMethod("TryConvert", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

		private static readonly ClaimsIdentityConverter _default = new ClaimsIdentityConverter(ClaimsIdentityConverter.GetDefaultConverters());

		private readonly Func<IIdentity, ClaimsIdentity>[] _converters;

		public static ClaimsIdentityConverter Default
		{
			get
			{
				return ClaimsIdentityConverter._default;
			}
		}

		internal ClaimsIdentityConverter(Func<IIdentity, ClaimsIdentity>[] converters)
		{
			this._converters = converters;
		}

		private static bool IsGrandfatheredIdentityType(IIdentity claimsIdentity)
		{
			return claimsIdentity is FormsIdentity || claimsIdentity is WindowsIdentity || claimsIdentity is GenericIdentity;
		}

		public ClaimsIdentity TryConvert(IIdentity identity)
		{
			if (ClaimsIdentityConverter.IsGrandfatheredIdentityType(identity))
			{
				return null;
			}
			for (int i = 0; i < this._converters.Length; i++)
			{
				ClaimsIdentity claimsIdentity = this._converters[i](identity);
				if (claimsIdentity != null)
				{
					return claimsIdentity;
				}
			}
			return null;
		}

		private static void AddToList(IList<Func<IIdentity, ClaimsIdentity>> converters, Type claimsIdentityType, Type claimType)
		{
			if (claimsIdentityType != null && claimType != null)
			{
				MethodInfo method = ClaimsIdentityConverter._claimsIdentityTryConvertOpenMethod.MakeGenericMethod(new Type[]
				{
					claimsIdentityType,
					claimType
				});
				Func<IIdentity, ClaimsIdentity> item = (Func<IIdentity, ClaimsIdentity>)Delegate.CreateDelegate(typeof(Func<IIdentity, ClaimsIdentity>), method);
				converters.Add(item);
			}
		}

		private static Func<IIdentity, ClaimsIdentity>[] GetDefaultConverters()
		{
			List<Func<IIdentity, ClaimsIdentity>> list = new List<Func<IIdentity, ClaimsIdentity>>();
			if (AppDomain.CurrentDomain.IsHomogenous && AppDomain.CurrentDomain.IsFullyTrusted)
			{
				Type type = Type.GetType("Microsoft.IdentityModel.Claims.IClaimsIdentity, Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
				Type type2 = Type.GetType("Microsoft.IdentityModel.Claims.Claim, Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
				ClaimsIdentityConverter.AddToList(list, type, type2);
			}
			Module module = typeof(object).Module;
			Type type3 = module.GetType("System.Security.Claims.ClaimsIdentity");
			Type type4 = module.GetType("System.Security.Claims.Claim");
			ClaimsIdentityConverter.AddToList(list, type3, type4);
			return list.ToArray();
		}
	}
}
