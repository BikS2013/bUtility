using bUtility.Reflection;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace bUtility.Dapper
{
    public static class DMLExtensionsOracle
    {
        public static int BulkInsertOracleDb<T>(this IDbConnection con, IEnumerable<T> dataList, int linesPerBatch = 1000, int timeout = 0, DMLOptions options = null)
        {
            if (!(con is OracleConnection))
            {
                throw new Exception("invalid connection");
            }
            int res = 0;
            if (dataList.HasAny())
            {
                var oracleCon = con as OracleConnection;
                if (oracleCon.State != ConnectionState.Open)
                {
                    oracleCon.Open();
                }
                using (var command = oracleCon.CreateCommand())
                {
                    command.CommandText = Statements<T>.GetInsert(options);
                    command.CommandType = CommandType.Text;
                    command.BindByName = true;
                    command.CommandTimeout = timeout;
                    var columns = typeof(T).GetMembers<PropertyInfo>();

                    int items = dataList.Count();
                    int outcount = 0;
                    while(outcount < items)
                    {
                        int incount =  linesPerBatch > items - outcount ? items - outcount : linesPerBatch;
                        command.ArrayBindCount = incount;
                        command.Parameters.Clear();
                        foreach (var c in columns)
                        {
                            command.Parameters.Add(c.Name.SetParameterDelimeter(options), c.PropertyType.ToOracleType(), dataList.Skip(outcount).Take(incount).Select(i => c.GetValue(i)).ToArray(), ParameterDirection.Input);
                        }
                        outcount += incount;
                        res += command.ExecuteNonQuery();
                    }
                }
            }
            return res;
        }

        //https://docs.oracle.com/cd/B19306_01/win.102/b14306/appendixa.htm
        //https://docs.oracle.com/cd/B19306_01/win.102/b14307/OracleDbTypeEnumerationType.htm#i1017320
        private static OracleDbType ToOracleType(this Type t)
        {
            if (t == typeof(string)) return OracleDbType.Varchar2;
            if (t.In(typeof(DateTime), typeof(DateTime?))) return OracleDbType.TimeStamp;
            if (t.In(typeof(bool), typeof(bool?))) return OracleDbType.Int16;
            if (t.In(typeof(decimal), typeof(decimal?))) return OracleDbType.Decimal;
            if (t.In(typeof(long), typeof(long?))) return OracleDbType.Int64;
            if (t.In(typeof(int), typeof(int?))) return OracleDbType.Int32;
            if (t.In(typeof(short), typeof(short?))) return OracleDbType.Int16;
            if (t.In(typeof(sbyte), typeof(sbyte?))) return OracleDbType.Byte;
            if (t.In(typeof(byte), typeof(byte?))) return OracleDbType.Int16;
            if (t.In(typeof(decimal), typeof(decimal?))) return OracleDbType.Decimal;
            if (t.In(typeof(float), typeof(float?))) return OracleDbType.Single;
            if (t.In(typeof(double), typeof(double?))) return OracleDbType.Double;
            if (t == typeof(byte[])) return OracleDbType.Blob;
            return default(OracleDbType);
        }
    }
}
