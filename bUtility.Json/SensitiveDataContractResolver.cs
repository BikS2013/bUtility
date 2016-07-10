using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace bUtility.Json
{
    public class SensitiveDataContractResolver : DefaultContractResolver
    {
        Func<string> SaltGenerator { get; set; }
        public SensitiveDataContractResolver(Func<string> saltGenerator)
        {
            SaltGenerator = saltGenerator;
        }
        private string GetEmittedPropertyName(JsonProperty originalProperty, SensitiveDataAttribute attribute)
        {
            return attribute.EmittedPropertyName.Clear() ?? $"{originalProperty.PropertyName}_encr";
        }


        JsonProperty GetJsonProperty(Type type, JsonProperty mappedProperty, SensitiveDataAttribute attribute)
        {
            return new JsonProperty()
            {
                DeclaringType = type,
                PropertyName = GetEmittedPropertyName(mappedProperty, attribute),
                UnderlyingName = mappedProperty.UnderlyingName,
                PropertyType = typeof(string),
                ValueProvider = new SensitiveDataValueProvider(mappedProperty.UnderlyingName, SaltGenerator),
                Readable = true,
            };
        }
        protected override IList<JsonProperty> CreateProperties(Type type, Newtonsoft.Json.MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
            if (properties.Count > 0)
            {
                foreach (var property in type.GetProperties())
                {
                    var sensitiveDataAttributes = property.GetCustomAttributes(typeof(SensitiveDataAttribute), false);
                    if (!sensitiveDataAttributes.Any()) { continue; }

                    var mappedProperty = properties.FirstOrDefault(p => p.UnderlyingName == property.Name);
                    if (mappedProperty != null)
                    {
                        var attribute = (sensitiveDataAttributes[0] as SensitiveDataAttribute);
                        if (attribute.Direction == Direction.Output)
                            properties.Add(GetJsonProperty(type, mappedProperty, attribute));
                        else
                        {
                            mappedProperty.ValueProvider = new SensitiveDataValueProvider(property.Name, SaltGenerator);
                        }
                    }
                }
            }
            return properties;
        }
    }
}
