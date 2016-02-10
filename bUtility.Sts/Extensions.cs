using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts.local
{
    public static class Extensions
    {
        static readonly JsonSerializerSettings defaultSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        public static string ToJson(this object obj)
        {
            if (obj == null) return null;
            return JsonConvert.SerializeObject(obj, defaultSettings);
        }
    }
}
