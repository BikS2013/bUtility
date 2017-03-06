using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace bUtility.Dapper.Test
{
    [TestClass]
    public class UnitTestSql
    {
        private static readonly string connectionstring = "Data Source=v000080038;Initial Catalog=Test;Integrated Security=true;";
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

            object updateObj = new { channel = "ch2", password = "psv2" };
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
            Assert.AreEqual(3, res.Count());

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
            DMLOptions.CurrentOptions = new DMLOptions("@", "[", "]");

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

            object updateObj = new { channel = "ch2", password = "psv2" };
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
            int rows = 11111;

            string channel = $"bitest_{new Random().Next(100000, 999999).ToString()}";
            List<testClass> list = new List<testClass>();
            for (int i = 0; i < rows; i++)
            {
                list.Add(Helper.GetNew(channel, i));
            }
            var res = dbc.BulkInsertSql(list);
            Assert.AreEqual(rows, res);

            //cleanup
            var delete = dbc.Delete<testClass>(new { channel = channel });
            Assert.AreEqual(rows, delete);
        }

        [TestMethod]
        public void TestSqlBulkInsertFiltered()
        {
            int rows = 123456;

            string channel = $"bitest_{new Random().Next(100000, 999999).ToString()}";
            List<testClass> list = new List<testClass>();
            for (int i = 0; i < rows; i++)
            {
                list.Add(Helper.GetNew(channel, i));
            }
            var res = dbc.BulkInsertSql(list, new List<string> { "password" });
            Assert.AreEqual(rows, res);

            var inserted = dbc.Select<testClass>(new { channel = channel });
            Assert.AreEqual(inserted.Count(), rows);
            Assert.IsNull(inserted.FirstOrDefault(r => r.password != null));

            //cleanup
            var delete = dbc.Delete<testClass>(new { channel = channel });
            Assert.AreEqual(rows, delete);
        }

        [TestMethod]
        public void TestFilteredInsert()
        {
            var id = Guid.NewGuid().ToString();

            var t1 = new testClass
            {
                channel = "ch1",
                username = "usr1",
                password = "psv1",
                sessionId = id
            };

            var username = typeof(testClass).GetProperty("username");
            var password = typeof(testClass).GetProperty("password");

            var res = dbc.Insert(t1, new List<PropertyInfo> { username, password });
            Assert.AreEqual(1, res);

            var inserted = dbc.SelectSingle<testClass>(new { sessionId = id });
            Assert.IsNotNull(inserted);
            Assert.IsTrue(inserted.channel.Equals("ch1"));
            Assert.IsNull(inserted.password);

            var del = dbc.Delete<testClass>(new { sessionId = id });
            Assert.AreEqual(del, 1);
        }

        [TestMethod]
        public void TestPeculiarInsert()
        {
            DMLOptions Toptions = DMLExtensionsSql.SqlDefaultOptions;

            string channel = Guid.NewGuid().ToString();

            var t1 = new PeculiarTestClass
            {
                channel = channel,
                index = 100
            };

            //default Insert is not working
            try
            {
                var res1 = dbc.Insert(t1);
                Assert.IsTrue(res1 == -1); //should not reach this code because index is a reserved word
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
            //should use options to use delimeters around identifiers (eg [index])
            try
            {
                var res1 = dbc.Insert(t1, options: Toptions);
                Assert.IsTrue(res1 == -1); //should not reach this code because id is a calculated column
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }

            var res = dbc.Insert(t1, new List<string> { "id" }, options: Toptions);
            Assert.AreEqual(1, res);

            var delete = dbc.Delete<testClass>(new { channel = channel, index = 100 }, options: Toptions);
            Assert.AreEqual(1, delete);
        }


        [TestMethod]
        public void TestCount()
        {
            int rows = 12345;

            string channel = $"bitest_{new Random().Next(100000, 999999).ToString()}";
            List<testClass> list = new List<testClass>();
            for (int i = 0; i < rows; i++)
            {
                list.Add(Helper.GetNew(channel, i));
            }
            var res = dbc.BulkInsertSql(list);
            Assert.AreEqual(rows, res);
            
            var count = dbc.Count<testClass>(new testClass { channel = channel, username = "us_1" });
            Assert.AreEqual(count, 1);

            count = dbc.Count<testClass>(new { channel = channel });
            Assert.AreEqual(count, rows);
            
            //cleanup
            var delete = dbc.Delete<testClass>(new { channel = channel });
            Assert.AreEqual(rows, delete);
        }

        [TestMethod]
        public void TestExecQuery()
        {
            int rows = 123;

            string channel = $"bitest_{new Random().Next(100000, 999999).ToString()}";
            List<testClass> list = new List<testClass>();
            for (int i = 0; i < rows; i++)
            {
                list.Add(Helper.GetNew(channel, i));
            }
            var res = dbc.BulkInsertSql(list);
            Assert.AreEqual(rows, res);

            var queried = dbc.execQuery<testClass>(new testClass { channel = channel });
            Assert.AreEqual(queried.Count(), rows);

            //cleanup
            var delete = dbc.Delete<testClass>(new { channel = channel });
            Assert.AreEqual(rows, delete);
        }
    }
}
