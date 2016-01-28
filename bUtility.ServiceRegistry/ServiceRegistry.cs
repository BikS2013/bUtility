using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility
{
    public static class ServiceRegistry
    {
        static Dictionary<Type, object> apiServices = new Dictionary<Type, object>();
        public static void RegisterService<T>(this T serviceImplementation)
        {
            apiServices[typeof(T)] = serviceImplementation;
        }
        public static S GetServiceImplementation<S>(this object controller) where S : class
        {
            if (!apiServices.ContainsKey(typeof(S))) return null;
            var serviceImplementation = apiServices[typeof(S)] as S;
            return serviceImplementation;
        }

        public static RSP GetDataServiceResponse<S, RSP>(this object controller, Func<S, RSP> serviceOperation)
            where S : class
        {
            S serviceImplementation = null;
            if (apiServices.ContainsKey(typeof(S))) { serviceImplementation = apiServices[typeof(S)] as S; }
            if (serviceImplementation == null) throw new Exception($"Service: {typeof(S)} not registered.");
            var data = serviceOperation(serviceImplementation);
            return data;
        }
        public static void GetDataServiceResponse<S>(this object controller, Action<S> serviceOperation)
            where S : class
        {
            var serviceImplementation = (apiServices.ContainsKey(typeof(S)) ? apiServices[typeof(S)] : null) as S;
            if (serviceImplementation == null) throw new Exception($"Service: {typeof(S)} not registered.");
            serviceOperation(serviceImplementation);
        }

    }
}
