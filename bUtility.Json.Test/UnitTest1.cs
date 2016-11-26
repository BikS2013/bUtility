using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bUtility.Json.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGenericDataContractResolver()
        {
            Case1 c1 = new Case1 { UserName = "user", Password = "password" };
            var text1 = Newtonsoft.Json.JsonConvert.SerializeObject(c1);

            Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
            settings.ContractResolver = new GenericDataContractResolver(new string[] { "password" });
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;

            var text2 = Newtonsoft.Json.JsonConvert.SerializeObject(c1, settings);

            var c2 = Newtonsoft.Json.JsonConvert.DeserializeObject<Case1>(text1, settings);
        }
    }
}
