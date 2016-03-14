using bUtility.Sts.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bUtility.Sts.MvcServer
{
    public static class Extensions
    {
        static string OnErrorRedirectUrl = "https://github.com/BikS2013/bUtility";
        public static IRelyingParty GetRelyingPartyElement(this string id, out ActionResult action)
        {
            IRelyingParty rp = StsConfiguration<RelyingParty>.Current.RelyingParties.FindByName(id);
            action = null;
            if (rp == null)
            {
                action = new RedirectResult(OnErrorRedirectUrl);
            }
            return rp;
        }

    }
}