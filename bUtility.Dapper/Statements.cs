using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Dapper
{
    public class Statements<T>
    {
        protected Statements() { }

        public static readonly string Table = typeof(T).Name;
        public static readonly string Columns = typeof(T).GetColumnList();
        public static readonly string Parameters = typeof(T).GetColumnList().GetParameterList();

        public static string GetSelect()
        {
            return $"select {Columns} from {Table}";
        }
        public static string GetSelect(object whereObject)
        {
            return $"select {Columns} from {Table} where {whereObject.GetWhereClause()}";
        }
        public static string GetSelect(string whereClause)
        {
            return $"select {Columns} from {Table} where {whereClause}";
        }
        public static string GetInsert()
        {
            return $"insert into {Table}({Columns}) values({Parameters})";
        }

        public static string GetUpdate(string updateClause, string whereClause)
        {
            return $"update {Table} set {updateClause} where {whereClause}";

        }
        public static string GetDelete(object whereObject)
        {
            return $"delete from {Table} where {whereObject.GetWhereClause()}";
        }

    }
}
