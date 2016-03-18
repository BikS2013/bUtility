using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT.InMemoryStore
{
    public static class Extensions
    {
        public static string Serialize(this object data)
        {
            if (data == null) return null;
            return JsonConvert.SerializeObject(data);
        }
    }
}
