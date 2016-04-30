using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;

namespace bUtility.WebApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class OverrideClaimsAuthorizationAttribute : Attribute
    {

    }
    public class ClaimsAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Αν υπάρχουν claims στο array θα πρέπει για κάθε ένα ο current user 
        /// να έχει claim με τον ίδιο τύπο και την ίδια τιμή.
        /// π.χ. όταν απαιτείται να έχει ένα συνδιασμό ρόλων για να έχει 
        /// access σε ένα action
        /// </summary>
        public Claim[] Claims { get; set; }
        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
        }
        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var attributes = actionContext.ActionDescriptor.GetCustomAttributes<OverrideClaimsAuthorizationAttribute>();
            if (attributes != null && attributes.Any())
            {
                return true;
            }

            bool isAuthorized = base.IsAuthorized(actionContext);
            if (!isAuthorized) return false;
            if (Claims != null)
            {
                var user = Thread.CurrentPrincipal.Identity as ClaimsIdentity;
                if (user == null)
                {
                    return false;
                }
                foreach (var claim in Claims)
                {
                    if (!user.HasClaim(claim.Type, claim.Value))
                    {
                        return false;
                    }
                }
            }
            return isAuthorized;
        }
    }
}
