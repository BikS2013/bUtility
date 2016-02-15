using bUtility.Sts.Configuration;
using bUtility.Sts.MvcSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bUtility.Sts.MvcSample.Controllers
{
    public class AccountController : Controller
    {
        static string OnErrorRedirectUrl = "https://github.com/BikS2013/bUtility";

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login( LoginModel model)
        {
            try
            {
                if (model.Id == null) model.Id = "test";

                ActionResult redirection; 
                RelyingParty rp = model.Id.GetRelyingPartyElement(out redirection);

                if (redirection != null)
                    return redirection;

                var result = this.HandleSignInRequestLocal(model, (m) => { return true; } , rp);
                if (result != null)
                    return result;
            }
            catch (Exception exception)
            {
                return new RedirectResult(OnErrorRedirectUrl);
            }

            return new EmptyResult();
        }
    }
}