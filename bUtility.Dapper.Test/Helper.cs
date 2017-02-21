using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Dapper.Test
{
    internal static class Helper
    {
        internal static testClass GetNew(string channel, int? next)
        {
            var random = next ?? new Random().Next(100000, 999999);
            return new testClass
            {
                sessionId = Guid.NewGuid().ToString(),
                channel = channel,
                username = $"us_{random}"
            };
        }
    }
}
