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


        public static string GetWhereClause4NotNulls(this object obj)
        {
            return obj?.GetMemberNames<PropertyInfo>((PropertyInfo pInfo) => pInfo.GetValue(obj) != null)?.Select(c => $"{c} = @{c}").Concatenate((c, n) => $"{c} and {n}");
        }

        public static string getWherePart4Nulls(this object obj)
        {
            return obj.GetMemberNames<PropertyInfo>((PropertyInfo pInfo) => pInfo.GetValue(obj) == null)?.Select(c => $"{c} is null")?.Concatenate((c, n) => $"{c} and {n}");
        }

        public static IEnumerable<string> GetWhereParts(this object obj)
        {
            var notNulls = obj.GetWhereClause4NotNulls();
            var nulls = obj.getWherePart4Nulls();
            if (notNulls != null) yield return notNulls;
            if (nulls != null) yield return nulls;
        }

        public static string getWherePart(this object obj)
        {
            return obj.GetMemberNames<PropertyInfo>().Select(c => $"{c} = @{c}").Aggregate((c, n) => $"{c} and {n}");
        }

        public static IEnumerable<T> execQuery<T>(this IDbConnection con, object param)
        {
            string tableName = typeof(T).Name;
            string selectPart = typeof(T).GetColumnList();
            string wherePart = param.getWherePart();
            return con.Query<T>($"select {selectPart} from {tableName} where {wherePart}", param);
        }
    }
}
