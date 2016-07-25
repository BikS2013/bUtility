using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using bUtility.Reflection;

namespace bUtility.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var t = new testClass
            {
                channel = "ch1",
                username = "usr1",
                password = "psv1",
                sessionId = Guid.NewGuid().ToString()
            };
            var fields = new string[] { "username", "password", "sessionId", "channel" };

            Dictionary<string, string> rqst = new Dictionary<string, string>();
            var sid = t.GetValue<string>("sessionId", null);
            fields.ToList().ForEach(f => rqst[f] = t.GetValue<string>(f, null));

        }
    }
}
