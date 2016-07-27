using bUtility.Reflection;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Dapper
{
    public static class DMLExtensions
    {
        public static string GetColumnList(this Type type)
        {
            return type.GetMemberNames<PropertyInfo>().Concatenate((c, n) => $"{c}, {n}");
        }
        public static string GetParameterList(this Type type)
        {
            return type.GetMemberNames<PropertyInfo>().Select(c => $"@{c.Trim()}").Concatenate((c, n) => $"{c}, {n}");
        }

        public static string GetParameterList(this string columnList)
        {
            return columnList.Split(',').Select(c => $"@{c.Trim()}").Concatenate((c, n) => $"{c}, {n}");
        }

        public static string GetUpdateClause(this Type type, Func<PropertyInfo, Boolean> filter = null)
        {
            return type.GetMemberNames<PropertyInfo>(filter).Select(c => $"{c}=@{c}").Concatenate((c, n) => $"{c}, {n}");
        }

        public static string GetWhereClause(this object obj, bool includeNulls = false)
        {
            var part1 = obj?.GetMemberNames<PropertyInfo>((PropertyInfo pInfo) => pInfo.GetValue(obj) != null)?
                .Select(c => $"{c} = @{c}").Concatenate((c, n) => $"{c} and {n}");
            if (!includeNulls) return part1;
            var part2 = obj.GetMemberNames<PropertyInfo>((PropertyInfo pInfo) => pInfo.GetValue(obj) == null)?.Select(c => $"{c} is null")?.Concatenate((c, n) => $"{c} and {n}");
            if (part1.Clear() != null && part2.Clear() != null) return $"{part1} and {part2}";
            return part1 ?? part2;
        }

        public static IEnumerable<T> execQuery<T>(this IDbConnection con, object param)
        {
            string tableName = typeof(T).Name;
            string selectPart = typeof(T).GetColumnList();
            string wherePart = param.GetWhereClause();
            return con.Query<T>($"select {selectPart} from {tableName} where {wherePart}", param);
        }

        public static IEnumerable<T> Select<T>(this IDbConnection con)
        {
            return con.Query<T>(Statements<T>.GetSelect());
        }
        public static T SelectSingle<T>(this IDbConnection con, object whereObject, IDbTransaction trn = null, bool buffered=true, int? timeout = 0, CommandType? commandType = null)
        {
            return con.Query<T>(Statements<T>.GetSelect(whereObject), whereObject, trn, buffered, timeout, commandType).FirstOrDefault();
        }
        public static IEnumerable<T> Select<T>(this IDbConnection con, object whereObject, IDbTransaction trn = null, bool buffered = true, int? timeout = 0, CommandType? commandType = null)
        {
            return con.Query<T>(Statements<T>.GetSelect(whereObject), whereObject, trn, buffered, timeout, commandType);
        }

        public static int Insert<T>(this IDbConnection con, T data, IDbTransaction trn=null, int? timeout=0, CommandType? commandType=null)
        {
            return con.Execute(Statements<T>.GetInsert(), data, trn, timeout, commandType);
        }

        public static int Delete<T>(this IDbConnection con, object whereObject, IDbTransaction trn = null, int? timeout = 0, CommandType? commandType = null)
        {
            return con.Execute(Statements<T>.GetDelete(whereObject), whereObject, trn, timeout, commandType);
        }
    }
}
