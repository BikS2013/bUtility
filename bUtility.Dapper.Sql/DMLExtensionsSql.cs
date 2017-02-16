using bUtility.Reflection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace bUtility.Dapper
{
    public static class DMLExtensionsSql
    {
        public static int BulkInsertSql<T>(this IDbConnection con, IEnumerable<T> dataList, int linesPerBatch = 1000, int timeout = 0, DMLOptions options = null)
        {
            if (!dataList.HasAny()) return 0;
            if (con is SqlConnection)
            {
                using (var bulkCopy = new SqlBulkCopy(con as SqlConnection))
                {
                    bulkCopy.DestinationTableName = Statements<T>.Table(options);
                    bulkCopy.BatchSize = linesPerBatch;
                    bulkCopy.BulkCopyTimeout = timeout;

                    var table = typeof(T).GetTable4BulkCopy(bulkCopy);
                    table.SetRows(dataList);

                    if (con.State != ConnectionState.Open) con.Open();
                    bulkCopy.WriteToServer(table);
                }
            }
            return dataList.Count();
        }

        public static DataTable GetTable4BulkCopy(this Type type, SqlBulkCopy bulkCopy)
        {
            var table = new DataTable();
            var columns = type.GetMembers<PropertyInfo>();
            foreach (var c in columns)
            {
                table.Columns.Add(c.Name, c.PropertyType);
                bulkCopy.ColumnMappings.Add(c.Name, c.Name);
            }
            return table;
        }

        public static void SetRows<T>(this DataTable table, IEnumerable<T> dataList)
        {
            foreach (var data in dataList)
            {
                table.Rows.Add(table.GetRow(data));
            }
        }

        public static DataRow GetRow<T>(this DataTable table, T data)
        {
            DataRow row = table.NewRow();
            var columns = typeof(T).GetMembers<PropertyInfo>();
            foreach (var c in columns)
            {
                row[c.Name] = c.GetValue(data);
            }
            return row;
        }
    }
}
