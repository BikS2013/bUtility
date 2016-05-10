using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;

namespace System.Web.Helpers.Claims
{
	internal abstract class ClaimsIdentity
	{
		private sealed class ClaimsIdentityImpl<TClaimsIdentity, TClaim> : ClaimsIdentity where TClaimsIdentity : class, IIdentity
		{
			private static readonly Func<TClaimsIdentity, IEnumerable<TClaim>> _claimsGetter = ClaimsIdentity.ClaimsIdentityImpl<TClaimsIdentity, TClaim>.CreateClaimsGetter();

			private readonly TClaimsIdentity _claimsIdentity;

			public ClaimsIdentityImpl(TClaimsIdentity claimsIdentity)
			{
				this._claimsIdentity = claimsIdentity;
			}

			private static Func<TClaimsIdentity, IEnumerable<TClaim>> CreateClaimsGetter()
			{
				PropertyInfo property = typeof(TClaimsIdentity).GetProperty("Claims", BindingFlags.Instance | BindingFlags.Public);
				MethodInfo getMethod = property.GetGetMethod();
				return (Func<TClaimsIdentity, IEnumerable<TClaim>>)Delegate.CreateDelegate(typeof(Func<TClaimsIdentity, IEnumerable<TClaim>>), getMethod);
			}

			public override IEnumerable<Claim> GetClaims()
			{
				return ClaimsIdentity.ClaimsIdentityImpl<TClaimsIdentity, TClaim>._claimsGetter(this._claimsIdentity).Select(new Func<TClaim, Claim>(Claim.Create<TClaim>));
			}
		}

		public abstract IEnumerable<Claim> GetClaims();

		internal static ClaimsIdentity TryConvert<TClaimsIdentity, TClaim>(IIdentity identity) where TClaimsIdentity : class, IIdentity
		{
			TClaimsIdentity tClaimsIdentity = identity as TClaimsIdentity;
			if (tClaimsIdentity == null)
			{
				return null;
			}
			return new ClaimsIdentity.ClaimsIdentityImpl<TClaimsIdentity, TClaim>(tClaimsIdentity);
		}
	}
}
