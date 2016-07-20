using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.WebApi
{
    public class ConfigurationProxy
    {
        public static string LoadString(string key)
        {
            return ConfigurationManager.AppSettings[key].Clear();
        }
        public static bool LoadBool(string key, bool defaultValue = false)
        {
            var configValue = ConfigurationManager.AppSettings[key];
            bool value;
            if (bool.TryParse(configValue, out value))
                return value;

            return defaultValue;
        }
        public static int LoadInt(string key, int defaultValue = 0)
        {
            var configValue = ConfigurationManager.AppSettings[key];
            int value;
            if (int.TryParse(configValue, out value))
                return value;

            return defaultValue;
        }

        public static TEnum LoadEnumValue<TEnum>(string enumValue)
            where TEnum : struct
        {
            var configValue = ConfigurationManager.AppSettings[enumValue];
            TEnum value;
            if (Enum.TryParse(configValue, true, out value))
                return value;

            if (String.IsNullOrEmpty(enumValue))
                throw new ConfigurationErrorsException("Null value not valid for " + typeof(TEnum) + " type");

            throw new ConfigurationErrorsException("Value " + enumValue + " not valid for " + typeof(TEnum) + " type");
        }
    }
}
