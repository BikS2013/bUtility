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
        public static readonly DMLOptions SqlDefaultOptions = new DMLOptions("@", "[", "]");

        public static int BulkInsertSql<T>(this IDbConnection con, IEnumerable<T> dataList, int linesPerBatch = 1000, int timeout = 0, DMLOptions options = null)
        {
            return con.BulkInsert(dataList, typeof(T).GetMembers<PropertyInfo>(), linesPerBatch, timeout, options);
        }

        public static int BulkInsertSql<T>(this IDbConnection con, IEnumerable<T> dataList, IEnumerable<string> excludedColumnNames, int linesPerBatch = 1000, int timeout = 0, DMLOptions options = null)
        {
            Func<PropertyInfo, Boolean> filter = (PropertyInfo pi) => { return !excludedColumnNames.Contains(pi.Name); };
            return con.BulkInsertSql(dataList, excludedColumnNames.HasAny() ? filter : null, linesPerBatch, timeout, options);
        }

        public static int BulkInsertSql<T>(this IDbConnection con, IEnumerable<T> dataList, IEnumerable<PropertyInfo> excludedColumns, int linesPerBatch = 1000, int timeout = 0, DMLOptions options = null)
        {
            Func<PropertyInfo, Boolean> filter = (PropertyInfo pi) => { return !excludedColumns.Contains(pi); };
            return con.BulkInsertSql(dataList, excludedColumns.HasAny() ? filter : null, linesPerBatch, timeout, options);
        }

        public static int BulkInsertSql<T>(this IDbConnection con, IEnumerable<T> dataList, Func<PropertyInfo, Boolean> columnfilter, int linesPerBatch = 1000, int timeout = 0, DMLOptions options = null)
        {
            var columns = typeof(T).GetMembers(columnfilter != null ? columnfilter : null);
            return con.BulkInsert(dataList, columns, linesPerBatch, timeout, options);
        }

        public static int BulkInsert<T>(this IDbConnection con, IEnumerable<T> dataList, IEnumerable<PropertyInfo> columns, int linesPerBatch, int timeout, DMLOptions options)
        {
            if (!(con is SqlConnection))
            {
                throw new Exception("invalid connection, SqlConnection expected");
            }
            if (!dataList.HasAny()) return 0;
            var curOptions = options ?? SqlDefaultOptions;
            using (var bulkCopy = new SqlBulkCopy(con as SqlConnection))
            {
                bulkCopy.DestinationTableName = Statements<T>.Table(curOptions);
                bulkCopy.BatchSize = linesPerBatch;
                bulkCopy.BulkCopyTimeout = timeout;

                var table = typeof(T).GetTable4BulkCopy(bulkCopy, columns);
                table.SetRows(dataList, columns);

                if (con.State != ConnectionState.Open) con.Open();
                bulkCopy.WriteToServer(table);
            }

            return dataList.Count();
        }

        private static DataTable GetTable4BulkCopy(this Type type, SqlBulkCopy bulkCopy, IEnumerable<PropertyInfo> columns)
        {
            var table = new DataTable();
            foreach (var c in columns)
            {
                table.Columns.Add(c.Name, c.PropertyType);
                bulkCopy.ColumnMappings.Add(c.Name, c.Name);
            }
            return table;
        }

        private static void SetRows<T>(this DataTable table, IEnumerable<T> dataList, IEnumerable<PropertyInfo> columns)
        {
            foreach (var data in dataList)
            {
                table.Rows.Add(table.GetRow(data, columns));
            }
        }

        private static DataRow GetRow<T>(this DataTable table, T data, IEnumerable<PropertyInfo> columns)
        {
            DataRow row = table.NewRow();
            foreach (var c in columns)
            {
                row[c.Name] = c.GetValue(data);
            }
            return row;
        }
    }
}
