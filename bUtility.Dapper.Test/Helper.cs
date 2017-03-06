using System;

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
                password = $"ps_{random}",
                username = $"us_{random}"
            };
        }
    }
}
