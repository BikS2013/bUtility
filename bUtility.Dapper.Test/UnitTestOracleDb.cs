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
        DMLOptions Options = new DMLOptions(':', "U_", '"');

        [TestMethod]
        public void TestOracleBulkInsert()
        {
            int rows = 100000;

            string channel = $"bitest_{DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")}";
            List<testClass> list = new List<testClass>();
            for (int i = 0; i < rows; i++)
            {
                list.Add(Helper.GetNew(channel, i));
            }
            var res = dbc.BulkInsertOracleDb(list, 50, options: Options);
            Assert.AreEqual(rows, res);
            
            //cleanup
            var delete = dbc.Delete<testClass>(new { PaymentId = channel }, options: Options);
            Assert.AreEqual(rows, delete);
        }
    }
}
