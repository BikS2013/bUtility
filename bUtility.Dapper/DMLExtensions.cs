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
        public static string SetIdentifierDelimeters(this string value, DMLOptions options = null)
        {
            var curOptions = options ?? DMLOptions.CurrentOptions;
            return curOptions.IdentifierStartingDelimeter + value.Trim() + curOptions.IdentifierEndingDelimeter;
        }
        public static string SetParameterDelimeter(this string value, DMLOptions options = null)
        {
            return (options ?? DMLOptions.CurrentOptions).ParameterDelimeter + value.Trim();
        }
        
        public static string SetUpdateParameterName(this string value, DMLOptions options = null)
        {
            return (options ?? DMLOptions.CurrentOptions).UpdateParameterDelimeter + value.Trim();
        }
        public static string SetUpdateParameterDelimeter(this string value, DMLOptions options = null)
        {
            return value.SetUpdateParameterName(options).SetParameterDelimeter(options);
        }

        public static string GetTableName(this Type type, DMLOptions options = null)
        {
            return type.Name.SetIdentifierDelimeters(options);
        }

        public static string GetColumnList(this Type type, DMLOptions options = null)
        {
            return type.GetMemberNames<PropertyInfo>().Select(c => c.SetIdentifierDelimeters(options)).Concatenate((c, n) => $"{c}, {n}");
        }

        public static string GetParameterList(this Type type, DMLOptions options = null)
        {
            return type.GetMemberNames<PropertyInfo>().Select(c => c.SetParameterDelimeter(options)).Concatenate((c, n) => $"{c}, {n}");
        }

        public static string GetParameterList(this string columnList, DMLOptions options = null)
        {
            return columnList.Split(',').Select(c => c.SetParameterDelimeter(options)).Concatenate((c, n) => $"{c}, {n}");
        }

        public static string GetUpdateClause(this Type type, Func<PropertyInfo, Boolean> filter = null, DMLOptions options = null)
        {
            return type.GetMemberNames<PropertyInfo>(filter).Select(c => $"{c}={c.SetParameterDelimeter(options)}").Concatenate((c, n) => $"{c}, {n}");
        }

        public static string GetUpdateClause(this object obj, bool includeNulls = false, DMLOptions options = null)
        {
            var part1 = obj?.GetMemberNames<PropertyInfo>((PropertyInfo pInfo) => pInfo.GetValue(obj) != null)?
                .Select(c => $"{c.SetIdentifierDelimeters(options)} = {c.SetUpdateParameterDelimeter(options)}").Concatenate((c, n) => $"{c} and {n}");
            if (!includeNulls) return part1;
            var part2 = obj.GetMemberNames<PropertyInfo>((PropertyInfo pInfo) => pInfo.GetValue(obj) == null)?.Select(c => $"{c.SetIdentifierDelimeters(options)} is null")?.Concatenate((c, n) => $"{c} and {n}");
            if (part1.Clear() != null && part2.Clear() != null) return $"{part1} and {part2}";
            return part1 ?? part2;
        }

        public static string GetWhereClause(this object obj, bool includeNulls = false, DMLOptions options = null)
        {
            var part1 = obj?.GetMemberNames<PropertyInfo>((PropertyInfo pInfo) => pInfo.GetValue(obj) != null)?
                .Select(c => $"{c.SetIdentifierDelimeters(options)} = {c.SetParameterDelimeter(options)}").Concatenate((c, n) => $"{c} and {n}");
            if (!includeNulls) return part1;
            var part2 = obj.GetMemberNames<PropertyInfo>((PropertyInfo pInfo) => pInfo.GetValue(obj) == null)?.Select(c => $"{c.SetIdentifierDelimeters(options)} is null")?.Concatenate((c, n) => $"{c} and {n}");
            if (part1.Clear() != null && part2.Clear() != null) return $"{part1} and {part2}";
            return part1 ?? part2;
        }

        public static IEnumerable<T> execQuery<T>(this IDbConnection con, object param, DMLOptions options = null)
        {
            string tableName = typeof(T).GetTableName(options);
            string selectPart = typeof(T).GetColumnList(options);
            string wherePart = param.GetWhereClause(options: options);
            return con.Query<T>($"select {selectPart} from {tableName} where {wherePart}", param);
        }

        public static IEnumerable<T> Select<T>(this IDbConnection con, DMLOptions options = null)
        {
            return con.Query<T>(Statements<T>.GetSelect(options));
        }

        public static T SelectSingle<T>(this IDbConnection con, object whereObject, IDbTransaction trn = null, bool buffered = true, int? timeout = 0, CommandType? commandType = null, DMLOptions options = null)
        {
            return con.Query<T>(Statements<T>.GetSelect(whereObject, options), whereObject, trn, buffered, timeout, commandType).FirstOrDefault();
        }

        public static IEnumerable<T> Select<T>(this IDbConnection con, object whereObject, IDbTransaction trn = null, bool buffered = true, int? timeout = 0, CommandType? commandType = null, DMLOptions options = null)
        {
            return con.Query<T>(Statements<T>.GetSelect(whereObject, options), whereObject, trn, buffered, timeout, commandType);
        }

        public static int Insert<T>(this IDbConnection con, T data, IDbTransaction trn = null, int? timeout = 0, CommandType? commandType = null, DMLOptions options = null)
        {
            return con.Execute(Statements<T>.GetInsert(options), data, trn, timeout, commandType);
        }

        public static int Delete<T>(this IDbConnection con, object whereObject, IDbTransaction trn = null, int? timeout = 0, CommandType? commandType = null, DMLOptions options = null)
        {
            return con.Execute(Statements<T>.GetDelete(whereObject, options), whereObject, trn, timeout, commandType);
        }

        public static int Update<T>(this IDbConnection con, object updateObject, object whereObject, IDbTransaction trn = null, int? timeout = 0, CommandType? commandType = null, DMLOptions options = null)
        {
            return con.Execute(Statements<T>.GetUpdate(updateObject, whereObject, options), GetMergedDynamicParams(updateObject, whereObject), trn, timeout, commandType);
        }

        public static DynamicParameters GetMergedDynamicParams(object updateObject, object whereObject, bool keepNulls = false, DMLOptions options = null)
        {
            if (updateObject == null && whereObject == null) return null;
            var pars = new DynamicParameters();
            Dictionary<string, object> members = new Dictionary<string, object>();
            updateObject?.GetMembers<PropertyInfo>(pi => { return keepNulls || pi.GetValue(updateObject) != null; }).ToList().ForEach(pi => pars.Add(pi.Name.SetUpdateParameterName(options), pi.GetValue(updateObject)));
            whereObject?.GetMembers<PropertyInfo>(pi => { return keepNulls || pi.GetValue(whereObject) != null; }).ToList().ForEach(pi => pars.Add(pi.Name, pi.GetValue(whereObject)));
            return pars;
        }
        /// <summary>
        /// Not to be used for many objects due to performance issues
        /// </summary>
        public static int MultipleInsert<T>(this IDbConnection con, IEnumerable<T> dataList, IDbTransaction trn = null, int? timeout = 0, CommandType? commandType = null, DMLOptions options = null)
        {
            return con.Execute(Statements<T>.GetInsert(options), dataList, trn, timeout, commandType);
        }
    }
}
