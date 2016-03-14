using bUtility.Sts.Configuration;
using bUtility.Sts.MvcServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace bUtility.Sts.MvcServer.Controllers
{
    public class AccountController : Controller
    {
        static string OnErrorRedirectUrl = "https://github.com/BikS2013/bUtility";
        // GET: Account
        public ActionResult Index(string id)
        {
            if (id == null) id = "test";
            return View( new LoginModel { Id = id} );
        }

        ClaimsPrincipal GetPrincipal(LoginModel model)
        {
            ClaimsIdentity i = new ClaimsIdentity(
                new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName )
                },
                "simpleSts");

            ClaimsPrincipal p = new ClaimsPrincipal(i);
            return p;
        }

        void ClearAllCookies()
        {
            foreach (var cookieName in Request.Cookies.Cast<string>().ToArray())
            {
                HttpContext.Response.Cookies.Add(new HttpCookie(cookieName) { Expires = new DateTime(1990, 1, 1) });
            }
        }

        ActionResult HandleSignInRequestLocal(Models.LoginModel model,
            Func<Models.LoginModel, bool> logonFunction, IRelyingParty rp)
        {
            try
            {

                var actualPrincipal = GetPrincipal(model);

                ClearAllCookies(); //except [culture, theme, font, etc] cookies

                System.Web.HttpContext.Current.Response.HandleSignIn(Request.Url, rp, actualPrincipal);

                return new EmptyResult();
            }
            catch (Exception ex)
            {
                throw new Exception("HandleSingleInRequest Failed. ", ex);
            }
        }

        public ActionResult Login(LoginModel model)
        {
            try
            {
                if (model.Id == null) model.Id = "test";



                ActionResult redirection;
                IRelyingParty rp = model.Id.GetRelyingPartyElement(out redirection);

                if (redirection != null)
                    return redirection;

                var result = HandleSignInRequestLocal(model, (m) => { return true; }, rp);
                if (result != null)
                    return result;
            }
            catch
            {
                return new RedirectResult(OnErrorRedirectUrl);
            }

            return new EmptyResult();
        }
    }
}