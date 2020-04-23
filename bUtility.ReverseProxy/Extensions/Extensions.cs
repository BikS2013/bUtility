using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace bUtility.ReverseProxy
{
    internal static class Extensions
    {
        internal static string ReplaceInsensitive(this string str, string from, string to)
        {
            return Regex.Replace(str, from, to, RegexOptions.IgnoreCase); 
        }

        internal static HttpClient TryGetClient(this Dictionary<string, HttpClient> clientProvider, 
            string clientName)
        {
            if (!clientProvider.TryGetValue(clientName, out HttpClient client))
                throw new ArgumentNullException(clientName, "Could not retrieve HttpClient");
            return client;
        }
    }
}
