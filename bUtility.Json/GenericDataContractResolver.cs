using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace bUtility.Json
{
    public class GenericDataContractResolver : DefaultContractResolver
    {
        IEnumerable<string> SensitiveProperties { get; set; }
        public GenericDataContractResolver(IEnumerable<string> sensitiveProperties)
        {
            SensitiveProperties = sensitiveProperties;
        }
        protected override IValueProvider CreateMemberValueProvider(System.Reflection.MemberInfo member)
        {
            return new GenericDataValueProvider(member, base.CreateMemberValueProvider(member), SensitiveProperties);
        }
    }
}
