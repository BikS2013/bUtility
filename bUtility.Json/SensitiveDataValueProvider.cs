using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace bUtility.Json
{
    public class SensitiveDataValueProvider : IValueProvider
    {
        public SensitiveDataValueProvider(string propertyName, Func<string> salt)
        {
            this.PropertyName = propertyName;
            this.SaltGenerator = salt;
        }

        string PropertyName { get; set; }
        private Func<string> SaltGenerator { get; set; }
        public object GetValue(object target)
        {
            var property = this.GetProperty(target);
            if (property == null)
                return null;

            var value = property.GetValue(target);
            return value == null ? null : Encrypt(value.ToString());
        }

        public void SetValue(object target, object value)
        {
            var property = this.GetProperty(target);
            if (property == null)
                return;

            if (value == null)
                property.SetValue(target, value);

            var realValue = Decrypt(value.ToString());
            property.SetValue(target, Convert.ChangeType(realValue, property.PropertyType));
        }

        private string Encrypt(string data)
        {
#warning τι γίνεται όταν λειτουργούμε σε φάρμα;
            var sensData =
                HttpServerUtility.UrlTokenEncode(
                System.Web.Security.MachineKey.Protect(
                System.Text.Encoding.UTF8.GetBytes(this.SaltGenerator() + data)));
            return sensData;
        }

        private string Decrypt(string data)
        {
            try
            {
                var value =
                    System.Text.Encoding.UTF8.GetString(
                    System.Web.Security.MachineKey.Unprotect(
                    HttpServerUtility.UrlTokenDecode(data)));

                var salt = this.SaltGenerator();
                if (!value.StartsWith(salt))
                    throw new SecurityException();

                value = value.Substring(salt.Length);
                return value;
            }
            catch (FormatException)
            {
                return data;
            }
            catch (CryptographicException)
            {
                return data;
            }
        }

        private System.Reflection.PropertyInfo GetProperty(object target)
        {
            if (target == null)
                return null;

            var property = target.GetType().GetProperty(this.PropertyName);
            if (property == null)
                return null;

            return property;
        }
    }
}
