using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Reflection
{
    public static partial class ReflectionExtensions
    {
        public static object GetInstance(this Type type, params object[] parameters)
        {
            var constructor = type.GetConstructor(System.Type.EmptyTypes);
            if (constructor != null)
            {
                var instance = constructor.Invoke(parameters);
                return instance;
            }
            return null;
        }

        public static object GetPublic(this object obj, string propertyName)
        {
            if (obj != null)
            {
                try
                {
                    var pinfo = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                    if (pinfo != null)
                    {
                        return pinfo.GetValue(obj);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception( $"failed to retrieve property {propertyName} from Type:{obj.GetType().Name} ", ex);
                }
            }
            return null;
        }

        public static T GetPublic<T>(this object obj, string propertyName, T defaultValue) where T : class
        {
            return GetPublic(obj, propertyName) as T ?? defaultValue;
        }

        public static void SetPublic(this object obj, string propertyName, object value)
        {
            if (obj != null)
            {
                try
                {
                    var pinfo = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                    if (pinfo != null)
                    {
                        pinfo.SetValue(obj, value);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Type: " + obj.GetType().Name + " PropertyName: " + propertyName, ex);
                }
            }
        }

        public static void Invoke(this object obj, string methodName, params object[] parameters)
        {
            if (obj != null)
            {
                try
                {
                    var minfo = obj.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                    if (minfo != null)
                    {
                        minfo.Invoke(obj, parameters);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Type: " + obj.GetType().Name + " MethodName: " + methodName, ex);
                }
            }
        }

        public static T GetCustomAttribute<T>(this MethodInfo methodInfo) where T : System.Attribute
        {
            if (methodInfo == null) return null;
            var attributes = methodInfo.GetCustomAttributes<T>();
            if (attributes != null) return attributes.FirstOrDefault(i => true);
            return null;
        }

    }
}
