using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts
{
    public class Federation
    {
        public enum action
        {
            attribute,
            pseudonym,
            signIn,
            signOut,
            signOutCleanup
        }

        public static readonly Dictionary<action, string> Actions = new Dictionary<action, string>
        {
            [action.attribute] = "wattr1.0",
            [action.pseudonym] = "wpseudo1.0",
            [action.signIn] = "wsignin1.0",
            [action.signOut] = "wsignout1.0",
            [action.signOutCleanup] = "wsignoutcleanup1.0"
        };

        public enum field
        {
            action,
            application,
            attribute,
            attributePtr,
            authenticationType,
            context,
            currentTime,
            encoding,
            federation,
            freshness,
            homeRealm,
            policy,
            pseudonym,
            pseudonymPtr,
            realm,
            reply,
            request,
            requestPtr,
            resource,
            result,
            resultPtr
        }
        public static readonly Dictionary<field, string> Fields = new Dictionary<field, string>
        {
            [field.action]= "wa",
            [field.application] = "wapp",
            [field.attribute] = "wattr",
            [field.attributePtr] = "wattrptr",
            [field.authenticationType] = "wauth",
            [field.context] = "wctx",
            [field.currentTime] = "wct",
            [field.encoding] = "wencoding",
            [field.federation] = "wfed",
            [field.freshness] = "wfresh",
            [field.homeRealm] = "whr",
            [field.policy] = "wp",
            [field.pseudonym] = "wpseudo",
            [field.pseudonymPtr] = "wpseudoptr",
            [field.realm] = "wtrealm",
            [field.reply] = "wreply",
            [field.request] = "wreq",
            [field.requestPtr] = "wreqptr",
            [field.resource] = "wres",
            [field.result] = "wresult",
            [field.resultPtr] = "wresultptr"
        };

    }
}
