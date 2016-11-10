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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">
        /// the result is false for null value
        /// </param>
        /// <param name="list">
        /// the result is false for null or empty list
        /// </param>
        /// <returns></returns>
        public static bool In<T>(this T value, IEnumerable<T> list)
        {
            if (!list.HasAny() || value == null) return false;
            var found = list.Contains(value);
            return found;
        }

        public static bool In<T>(this T? value, IEnumerable<T> list) where T : struct
        {
            if (value == null) return false;
            return value.Value.In(list);
        }

        public static bool In<T>(this T value, params T[] list)
        {
            return value.In((IEnumerable<T>)list);
        }

        public static bool In<T>(this T? value, params T[] list) where T : struct
        {
            return value.In((IEnumerable<T>)list);
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
