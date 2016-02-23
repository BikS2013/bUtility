using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts.CommandLine
{
    public class JsonHelper
    {
        JsonSerializerSettings _settings;
        JsonSerializerSettings getSettings()
        {
            if (_settings == null)
            {
                _settings = new Newtonsoft.Json.JsonSerializerSettings();
                _settings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                _settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            }
            return _settings;
        }
        public bool TrySerializeObjectToString(Object value, out string serValue)
        {
            Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();

            const Formatting formatting = Newtonsoft.Json.Formatting.None;
            try
            {
                serValue = JsonConvert.SerializeObject(value, formatting, getSettings());
                return true;
            }
            catch (Exception)
            {
                serValue = null;
                return false;
            }
        }


        public bool TryDeserializeObjectFromString<T>(string serValue, out T value) where T : class
        {
            try
            {
                value = JsonConvert.DeserializeObject(serValue, typeof(T), getSettings()) as T;
                return true;
            }
            catch (Exception)
            {
                value = null;
                return false;
            }
        }

    }
}
