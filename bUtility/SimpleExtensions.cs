using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility
{
    public static partial class SimpleExtensions
    {
        public static string Clear(this string value)
        {
            if (value == null)
                return null;
            if (value.Trim() == "")
                return null;
            return value.Trim();
        }

        public static bool In<T>(this T value, params T[] list)
        {
            if (list == null || list.Length == 0 || value == null) return false;
            var found = list.Contains(value);
            return found;
        }

        public static bool HasAny<T>(this IEnumerable<T> collection)
        {
            return collection != null && collection.Any();
        }

        public static string Concatenate(this IEnumerable<string> list, Func<string, string, string> pattern)
        {
            if (list.HasAny())
            {
                return list.Aggregate((c, n) => pattern(c, n));
            }
            return null;
        }
    }
}
