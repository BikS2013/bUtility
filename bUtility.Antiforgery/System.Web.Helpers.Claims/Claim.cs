using System;
using System.Reflection;

namespace System.Web.Helpers.Claims
{
	internal sealed class Claim
	{
		private static class ClaimFactory<TClaim>
		{
			private static readonly Func<TClaim, string> _claimTypeGetter = Claim.ClaimFactory<TClaim>.CreateClaimTypeGetter();

			private static readonly Func<TClaim, string> _valueGetter = Claim.ClaimFactory<TClaim>.CreateValueGetter();

			public static Claim Create(TClaim claim)
			{
				return new Claim(Claim.ClaimFactory<TClaim>._claimTypeGetter(claim), Claim.ClaimFactory<TClaim>._valueGetter(claim));
			}

			private static Func<TClaim, string> CreateClaimTypeGetter()
			{
				return Claim.ClaimFactory<TClaim>.CreateGeneralPropertyGetter("ClaimType") ?? Claim.ClaimFactory<TClaim>.CreateGeneralPropertyGetter("Type");
			}

			private static Func<TClaim, string> CreateGeneralPropertyGetter(string propertyName)
			{
				PropertyInfo property = typeof(TClaim).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public, null, typeof(string), Type.EmptyTypes, null);
				if (property == null)
				{
					return null;
				}
				MethodInfo getMethod = property.GetGetMethod();
				return (Func<TClaim, string>)Delegate.CreateDelegate(typeof(Func<TClaim, string>), getMethod);
			}

			private static Func<TClaim, string> CreateValueGetter()
			{
				return Claim.ClaimFactory<TClaim>.CreateGeneralPropertyGetter("Value");
			}
		}

		public string ClaimType
		{
			get;
			private set;
		}

		public string Value
		{
			get;
			private set;
		}

		public Claim(string claimType, string value)
		{
			this.ClaimType = claimType;
			this.Value = value;
		}

		internal static Claim Create<TClaim>(TClaim claim)
		{
			return Claim.ClaimFactory<TClaim>.Create(claim);
		}
	}
}
