using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace bUtility.CookieHandler
{
    public sealed class SecureChunkedCookieHandler: ChunkedCookieHandler
    {
        internal override void WriteInternal(byte[] value, string name, string path, string domain, DateTime expirationTime, bool secure, bool httpOnly, HttpCookieCollection requestCookies, HttpCookieCollection responseCookies)
        {
            string cookieValue = Convert.ToBase64String(value);
            this.DeleteInternal(name, path, domain, requestCookies, responseCookies);
            foreach (KeyValuePair<string, string> current in this.GetCookieChunks(name, cookieValue))
            {
                HttpCookie httpCookie = new HttpCookie(current.Key, current.Value);
                httpCookie.Secure = true;
                httpCookie.HttpOnly = httpOnly;
                httpCookie.Path = path;
                if (!string.IsNullOrEmpty(domain))
                {
                    httpCookie.Domain = domain;
                }
                if (expirationTime != DateTime.MinValue)
                {
                    httpCookie.Expires = expirationTime;
                }
                responseCookies.Set(httpCookie);
                //if (DiagnosticUtility.ShouldTrace(TraceEventType.Information))
                //{
                //    TraceEventType arg_A6_0 = TraceEventType.Information;
                //    int arg_A6_1 = 786438;
                //    string arg_A6_2 = null;
                //    ChunkedCookieHandlerTraceRecord.Action arg_A0_0 = ChunkedCookieHandlerTraceRecord.Action.Writing;
                //    HttpCookie expr_9A = httpCookie;
                //    TraceUtility.TraceEvent(arg_A6_0, arg_A6_1, arg_A6_2, new ChunkedCookieHandlerTraceRecord(arg_A0_0, expr_9A, expr_9A.Path), null);
                //}
            }
        }

    }
}
