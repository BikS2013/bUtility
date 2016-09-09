using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace bUtility.CookieHandler
{
    public abstract class ChunkedCookieHandler : System.IdentityModel.Services.CookieHandler
    {
        public const int DefaultChunkSize = 2000;

        public const int MinimumChunkSize = 1000;

        private int _chunkSize;

        public int ChunkSize
        {
            get
            {
                return this._chunkSize;
            }
        }

        public ChunkedCookieHandler() : this(2000)
        {
        }

        public ChunkedCookieHandler(int chunkSize)
        {
            if (chunkSize < 1000)
            {
                throw new ArgumentOutOfRangeException("chunkSize", "ID1016: 1000");
            }
            this._chunkSize = chunkSize;
        }

        protected override void DeleteCore(string name, string path, string domain, HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this.DeleteInternal(name, path, domain, context.Request.Cookies, context.Response.Cookies);
        }

        internal void DeleteInternal(string name, string path, string domain, HttpCookieCollection requestCookies, HttpCookieCollection responseCookies)
        {
            foreach (HttpCookie current in this.GetCookieChunks(name, requestCookies))
            {
                HttpCookie httpCookie = new HttpCookie(current.Name, null);
                httpCookie.Path = path;
                httpCookie.Expires = DateTime.UtcNow.AddDays(-1.0);
                if (!string.IsNullOrEmpty(domain))
                {
                    httpCookie.Domain = domain;
                }
                //if (DiagnosticUtility.ShouldTrace(TraceEventType.Information))
                //{
                //    TraceUtility.TraceEvent(TraceEventType.Information, 786438, null, new ChunkedCookieHandlerTraceRecord(ChunkedCookieHandlerTraceRecord.Action.Deleting, current, path), null);
                //}
                responseCookies.Set(httpCookie);
            }
        }

        // System.IdentityModel.CryptoHelper
        public static int CeilingDivide(int dividend, int divisor)
        {
            int arg_08_0 = dividend % divisor;
            int num = dividend / divisor;
            if (arg_08_0 > 0)
            {
                num++;
            }
            return num;
        }


        internal IEnumerable<KeyValuePair<string, string>> GetCookieChunks(string baseName, string cookieValue)
        {
            int num = CeilingDivide(cookieValue.Length, this._chunkSize);
            //if (num > 20 && DiagnosticUtility.ShouldTrace(TraceEventType.Warning))
            //{
            //    TraceUtility.TraceString(TraceEventType.Warning, SR.GetString("ID8008"), new object[0]);
            //}
            int num2;
            for (int i = 0; i < num; i = num2 + 1)
            {
                yield return new KeyValuePair<string, string>(
                    ChunkedCookieHandler.GetChunkName(baseName, i), 
                    cookieValue.Substring(i * this._chunkSize, Math.Min(cookieValue.Length - i * this._chunkSize, this._chunkSize)));
                num2 = i;
            }
            yield break;
        }

        private IEnumerable<HttpCookie> GetCookieChunks(string baseName, HttpCookieCollection cookies)
        {
            int num = 0;
            string chunkName = ChunkedCookieHandler.GetChunkName(baseName, num);
            HttpCookie httpCookie;
            while ((httpCookie = cookies[chunkName]) != null)
            {
                yield return httpCookie;
                int num2 = num + 1;
                num = num2;
                chunkName = ChunkedCookieHandler.GetChunkName(baseName, num2);
            }
            yield break;
        }

        protected override byte[] ReadCore(string name, HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            return this.ReadInternal(name, context.Request.Cookies);
        }

        internal byte[] ReadInternal(string name, HttpCookieCollection requestCookies)
        {
            StringBuilder stringBuilder = null;
            foreach (HttpCookie current in this.GetCookieChunks(name, requestCookies))
            {
                if (stringBuilder == null)
                {
                    stringBuilder = new StringBuilder();
                }
                stringBuilder.Append(current.Value);
                //if (DiagnosticUtility.ShouldTrace(TraceEventType.Information))
                //{
                //    TraceEventType arg_4C_0 = TraceEventType.Information;
                //    int arg_4C_1 = 786438;
                //    string arg_4C_2 = null;
                //    ChunkedCookieHandlerTraceRecord.Action arg_46_0 = ChunkedCookieHandlerTraceRecord.Action.Reading;
                //    HttpCookie expr_40 = current;
                //    TraceUtility.TraceEvent(arg_4C_0, arg_4C_1, arg_4C_2, new ChunkedCookieHandlerTraceRecord(arg_46_0, expr_40, expr_40.Path), null);
                //}
            }
            if (stringBuilder != null)
            {
                return Convert.FromBase64String(stringBuilder.ToString());
            }
            return null;
        }

        protected override void WriteCore(byte[] value, string name, string path, string domain, DateTime expirationTime, bool secure, bool httpOnly, HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this.WriteInternal(value, name, path, domain, expirationTime, secure, httpOnly, context.Request.Cookies, context.Response.Cookies);
        }

        internal abstract void WriteInternal(byte[] value, string name, string path, string domain, DateTime expirationTime, bool secure, bool httpOnly, HttpCookieCollection requestCookies, HttpCookieCollection responseCookies);
        //internal void WriteInternal(byte[] value, string name, string path, string domain, DateTime expirationTime, bool secure, bool httpOnly, HttpCookieCollection requestCookies, HttpCookieCollection responseCookies)
        //{
        //    string cookieValue = Convert.ToBase64String(value);
        //    this.DeleteInternal(name, path, domain, requestCookies, responseCookies);
        //    foreach (KeyValuePair<string, string> current in this.GetCookieChunks(name, cookieValue))
        //    {
        //        HttpCookie httpCookie = new HttpCookie(current.Key, current.Value);
        //        httpCookie.Secure = secure;
        //        httpCookie.HttpOnly = httpOnly;
        //        httpCookie.Path = path;
        //        if (!string.IsNullOrEmpty(domain))
        //        {
        //            httpCookie.Domain = domain;
        //        }
        //        if (expirationTime != DateTime.MinValue)
        //        {
        //            httpCookie.Expires = expirationTime;
        //        }
        //        responseCookies.Set(httpCookie);
        //        //if (DiagnosticUtility.ShouldTrace(TraceEventType.Information))
        //        //{
        //        //    TraceEventType arg_A6_0 = TraceEventType.Information;
        //        //    int arg_A6_1 = 786438;
        //        //    string arg_A6_2 = null;
        //        //    ChunkedCookieHandlerTraceRecord.Action arg_A0_0 = ChunkedCookieHandlerTraceRecord.Action.Writing;
        //        //    HttpCookie expr_9A = httpCookie;
        //        //    TraceUtility.TraceEvent(arg_A6_0, arg_A6_1, arg_A6_2, new ChunkedCookieHandlerTraceRecord(arg_A0_0, expr_9A, expr_9A.Path), null);
        //        //}
        //    }
        //}

        private static string GetChunkName(string baseName, int chunkIndex)
        {
            if (chunkIndex != 0)
            {
                return baseName + chunkIndex.ToString(CultureInfo.InvariantCulture);
            }
            return baseName;
        }
    }
}
