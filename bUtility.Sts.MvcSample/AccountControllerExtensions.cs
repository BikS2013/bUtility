using bUtility.Sts.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace bUtility.Sts.MvcSample
{
    public static class AccountControllerExtensions
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


        static void ClearAllCookies(Controller controller)
        {
            foreach (var cookieName in controller.Request.Cookies.Cast<string>().ToArray())
            {
                controller.HttpContext.Response.Cookies.Add(new HttpCookie(cookieName) { Expires = new DateTime(1990, 1, 1) });
            }
        }


        static ClaimsPrincipal GetPrincipal()
        {
            ClaimsIdentity i = new ClaimsIdentity(
                new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "mitsos" )
                },
                "simpleSts");

            ClaimsPrincipal p = new ClaimsPrincipal(i);
            return p;
        }

        public static ActionResult HandleSignInRequestLocal(this Controller controller, Models.LoginModel model,
            Func<Models.LoginModel, bool> logonFunction, IRelyingParty rp)
        {
            try
            {
                //if (model.Action != WSFederationConstants.Actions.SignIn)
                //    return null;

                //ILogOnResponse logonResponse = null;

                //try
                //{

                //    logonResponse = logonFunction(model);
                //}
                //catch (Exception ex)
                //{
                //}

                //var redirection = logonResponse.chk4redirection(model.RelyingParty, model.Line1); //based on login error
                //if (redirection != null)
                //{
                //    return controller.ApplyRedirection(redirection);
                //}

                //logonResponse.ActualUserId = logonResponse.ActualUserId.Clear() ?? model.Line1;

                //var antiforgerytoken = controller.Request.Form.GetValues("__RequestVerificationToken").Nth();

                //var additionalClaims = model.RelyingParty.GetAdditionalClaims();

                //var actualPrincipal = new LoginHelper().GetPrincipal(logonResponse, antiforgerytoken, null, additionalClaims.ToArray());

                var actualPrincipal = GetPrincipal();

                ClearAllCookies(controller); //except [culture, theme, font, etc] cookies

                System.Web.HttpContext.Current.Response.HandleSignIn(controller.Request.Url, rp, actualPrincipal);

                return new EmptyResult();
            }
            catch (Exception ex)
            {
                throw new Exception("HandleSingleInRequest Failed. ", ex);
            }
        }


    }
}