using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;

namespace bUtility.Dapper.Test
{
    [TestClass]
    public class UnitTestOracleDb
    {
        private static readonly string connectionstring = "Data Source=;User Id=;Password=;";
        private static readonly IDbConnection dbc = new OracleConnection(connectionstring);
        DMLOptions Options = new DMLOptions(":", "\"");

        [TestMethod]
        public void TestOracleBulkInsert()
        {
            DMLOptions.CurrentOptions = DMLExtensionsOracle.OracleDefaultOptions;

            int rows = 100000;

            string channel = $"bitest_{DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")}";
            List<testClass> list = new List<testClass>();
            for (int i = 0; i < rows; i++)
            {
                list.Add(Helper.GetNew(channel, i));
            }
            var res = dbc.BulkInsertOracleDb(list, 50);
            Assert.AreEqual(rows, res);
            
            //cleanup
            var delete = dbc.Delete<testClass>(new { PaymentId = channel });
            Assert.AreEqual(rows, delete);
        }


        [TestMethod]
        public void TestUpdateOptions4OracleDb()
        {
            string session = Guid.NewGuid().ToString();

            var t1 = new testClass
            {
                channel = "ch1",
                username = "usr1",
                password = "psv1",
                sessionId = session
            };

            var res = dbc.Insert(t1, options: Options);
            Assert.AreEqual(1, res);

            object updateObj = new { channel = "ch2" };
            object whereObj = new { sessionId = session, channel = "ch1" };

            res = dbc.Update<testClass>(updateObj, whereObj, options: Options);
            Assert.AreEqual(1, res);
        }

    }
}
