using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace bUtility.Dapper.Test
{
    [TestClass]
    public class UnitTestSql
    {
        private static readonly string connectionstring = "Data Source=;Initial Catalog=.Test;Integrated Security=true;";
        private static readonly IDbConnection dbc = new SqlConnection(connectionstring);

        [TestMethod]
        public void TestInsert()
        {
            var t1 = new testClass
            {
                channel = "ch1",
                username = "usr1",
                password = "psv1",
                sessionId = Guid.NewGuid().ToString()
            };

            var res = dbc.Insert(t1);
            Assert.AreEqual(1, res);
        }
        
        [TestMethod]
        public void TestUpdate()
        {
            string session = Guid.NewGuid().ToString();

            var t1 = new testClass
            {
                channel = "ch1",
                username = "usr1",
                password = "psv1",
                sessionId = session
            };

            var res = dbc.Insert(t1);
            Assert.AreEqual(1, res);

            object updateObj = new { channel = "ch2" };
            object whereObj = new { sessionId = session, channel = "ch1" };

            res = dbc.Update<testClass>(updateObj, whereObj);
            Assert.AreEqual(1, res);
        }

        [TestMethod]
        public void TestSelect()
        {
            string session = Guid.NewGuid().ToString();

            var t1 = new testClass
            {
                channel = "ch1",
                username = "usr1",
                password = "psv1",
                sessionId = session
            };
            var t2 = new testClass
            {
                channel = "ch2",
                username = "usr2",
                password = "psv2",
                sessionId = session
            };
            var t3 = new testClass
            {
                channel = "ch3",
                username = "usr3",
                password = "psv3",
                sessionId = session
            };

            dbc.Insert(t1);
            dbc.Insert(t2);
            dbc.Insert(t3);

            object whereObj = new { sessionId = session };
            var res = dbc.Select<testClass>(whereObj);
            Assert.IsNotNull(res);
            Assert.AreEqual(3, res.ToList().Count());

            object whereObj2 = new { sessionId = session, channel = "ch1" };
            var res2 = dbc.Select<testClass>(whereObj2);
            Assert.IsNotNull(res2);
            Assert.AreEqual(1, res2.Count());

            var res3 = dbc.Select<testClass>();
            Assert.IsNotNull(res3);
            Assert.IsTrue(res3.Count() >= 3);
        }

        [TestMethod]
        public void TestDelete()
        {
            string session = Guid.NewGuid().ToString();

            var t1 = new testClass
            {
                channel = "ch1",
                username = "usr1",
                password = "psv1",
                sessionId = session
            };

            var res = dbc.Insert(t1);
            Assert.AreEqual(1, res);
            
            object whereObj = new { sessionId = session, channel = "ch1" };

            res = dbc.Delete<testClass>(whereObj);
            Assert.AreEqual(1, res);
        }

        [TestMethod]
        public void TestUpdateOptions4SqlServer()
        {
            DMLOptions.CurrentOptions = new DMLOptions('@', "R0_", '[', ']');

            string session = Guid.NewGuid().ToString();

            var t1 = new testClass
            {
                channel = "ch1",
                username = "usr1",
                password = "psv1",
                sessionId = session
            };

            var res = dbc.Insert(t1, options: DMLOptions.DefaultOptions);
            Assert.AreEqual(1, res);

            object updateObj = new { channel = "ch2" };
            object whereObj = new { sessionId = session, channel = "ch1" };

            res = dbc.Update<testClass>(updateObj, whereObj);
            Assert.AreEqual(1, res);
        }

        [TestMethod]
        public void TestUpdateOptions4OracleDb()
        {
            DMLOptions.CurrentOptions = new DMLOptions(':', "U_", '"');

            string session = Guid.NewGuid().ToString();

            var t1 = new testClass
            {
                channel = "ch1",
                username = "usr1",
                password = "psv1",
                sessionId = session
            };

            var res = dbc.Insert(t1);
            Assert.AreEqual(1, res);

            object updateObj = new { channel = "ch2" };
            object whereObj = new { sessionId = session, channel = "ch1" };

            res = dbc.Update<testClass>(updateObj, whereObj);
            Assert.AreEqual(1, res);
        }

        [TestMethod]
        public void TestMultipleInsert()
        {
            int rows = 5;
            string channel = $"bitest_{new Random().Next(100000, 999999).ToString()}";
            List<testClass> list = new List<testClass>();
            for (int i = 0; i < rows; i++)
            {
                list.Add(Helper.GetNew(channel, i));
            }
            var res = dbc.MultipleInsert(list);
            Assert.AreEqual(rows, res);
            
            //cleanup
            var delete = dbc.Delete<testClass>(new { channel = channel });
            Assert.AreEqual(rows, delete);
        }

        [TestMethod]
        public void TestSqlBulkInsert()
        {
            int rows = 100000;
            
            string channel = $"bitest_{new Random().Next(100000, 999999).ToString()}";
            List<testClass> list = new List<testClass>();
            for(int i = 0; i < rows; i++)
            {
                list.Add(Helper.GetNew(channel, i));
            }
            var res = dbc.BulkInsertSql(list);
            Assert.AreEqual(rows, res);
            
            //cleanup
            var delete = dbc.Delete<testClass>(new { channel = channel });
            Assert.AreEqual(rows, delete);
        }
    }
}
