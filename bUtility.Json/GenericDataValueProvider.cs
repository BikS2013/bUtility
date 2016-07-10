using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace bUtility.Json
{
    public class GenericDataValueProvider : IValueProvider
    {
        public IEnumerable<string> SensitiveProperties { get; set; }

        public GenericDataValueProvider(System.Reflection.MemberInfo member, IValueProvider inner, IEnumerable<string> sensitiveProperties)
        {
            if (member == null) throw new ArgumentNullException("member");
            if (inner == null) throw new ArgumentNullException("inner");
            Member = member;
            Inner = inner;
            SensitiveProperties = sensitiveProperties?.Select(p => p.ToLowerInvariant());
        }
        IValueProvider Inner { get; set; }
        MemberInfo Member { get; set; }


        static string MaskValue(string str, bool full = false)
        {
            if (str == null) return null;

            if (str.Length == 0) return str;
            if (str.Length == 1) return "*";
            if (full) return new string('*', 8);

            var filler = new string('*', str.Length / 2);
            var firstLength = (str.Length - filler.Length) / 2;
            return str.Substring(0, firstLength) + filler + str.Substring(firstLength + filler.Length);

        }

        public object GetValue(object target)
        {
            var value = Inner.GetValue(target);
            if (value != null && value is string)
            {
                var name = this.Member.Name.ToLowerInvariant();
                if (SensitiveProperties?.FirstOrDefault(p => name.Contains(p)) != null)
                {
                    return MaskValue(value as string, true);
                }
            }
            return value;
        }

        public void SetValue(object target, object value)
        {
            Inner.SetValue(target, value);
        }
    }
}
