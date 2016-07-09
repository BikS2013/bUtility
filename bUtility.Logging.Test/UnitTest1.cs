using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bUtility.Logging.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestEventViewer()
        {
            var l = new Logger("bUtilitySource");
            l.Error("test logger 1");

            l.Error(new Exception("an exception sample"));
        }
    }
}
