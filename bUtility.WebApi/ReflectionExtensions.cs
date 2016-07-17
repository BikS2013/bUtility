using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace bUtility.WebApi
{
    public static class ReflectionExtensions
    {
        public static MethodInfo GetAction(this Type controllerType, string actionName)
        {
            if (controllerType != null)
            {
                var methods = controllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
                if (methods != null)
                {
                    foreach (var m in methods)
                    {
                        var attr = m.GetCustomAttribute(typeof(ActionNameAttribute));
                        if (attr != null && (attr as ActionNameAttribute).Name == actionName)
                        {
                            return m;
                        }
                    }
                }
            }
            return null;
        }
        public static void PropertyUpdate(this object obj, Action<object, PropertyInfo> updateAction)
        {
            if (obj == null) return;
            if (obj is IEnumerable)
            {
                foreach (object item in obj as IEnumerable)
                {
                    item.PropertyUpdate(updateAction);
                }
            }
            else {
                obj.GetType().GetProperties().ToList().ForEach(p =>
                {
                    Console.WriteLine(p.Name);
                    if (p.PropertyType.IsClass
                    && p.PropertyType != typeof(string)
                    && !p.PropertyType.IsArray)
                    {
                        var pObj = p.GetValue(obj);
                        if (pObj != null)
                        {
                            pObj.PropertyUpdate(updateAction);
                        }
                    }
                    else
                    {
                        updateAction(obj, p);
                    }
                });
            }
        }

        public static void ToLocalTime(this object obj)
        {
            obj.PropertyUpdate((o, p) => {
                if (p.PropertyType == typeof(DateTime))
                {
                    DateTime v = (DateTime)p.GetValue(o);
                    p.SetValue(o, v.ToLocalTime());
                }
                else if (p.PropertyType == typeof(DateTime?))
                {
                    DateTime? v = (DateTime?)p.GetValue(o);
                    if (v != null)
                    {
                        p.SetValue(o, v?.ToLocalTime());
                    }
                }
            });
        }

        public static void ToUniversalTime(this object obj)
        {
            obj.PropertyUpdate((o, p) => {
                if (p.PropertyType == typeof(DateTime))
                {
                    DateTime v = (DateTime)p.GetValue(o);
                    p.SetValue(o, v.ToUniversalTime());
                }
                else if (p.PropertyType == typeof(DateTime?))
                {
                    DateTime? v = (DateTime?)p.GetValue(o);
                    if (v != null)
                    {
                        p.SetValue(o, v?.ToUniversalTime());
                    }
                }
            });
        }

    }
}
