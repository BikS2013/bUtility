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
        
        public static string Table(DMLOptions options = null)
        {
            return typeof(T).GetTableName(options);
        }

        public static string Columns(DMLOptions options = null)
        {
            return typeof(T).GetColumnList(options);
        }

        public static string Parameters(DMLOptions options = null)
        {
            return typeof(T).GetParameterList(options);
        }

        public static string GetSelect(DMLOptions options = null)
        {
            return $"select {Columns(options)} from {Table(options)}";
        }

        public static string GetSelect(object whereObject, DMLOptions options = null)
        {
            return $"select {Columns(options)} from {Table(options)} where {whereObject.GetWhereClause(options: options)}";
        }
        
        public static string GetSelect(string whereClause, DMLOptions options = null)
        {
            return $"select {Columns(options)} from {Table(options)} where {whereClause}";
        }

        public static string GetCount(object whereObject, DMLOptions options = null)
        {
            return $"select count(*) as Found from {Table(options)} where {whereObject.GetWhereClause(options: options)}";
        }

        public static string GetCount(string whereClause, DMLOptions options = null)
        {
            return $"select count(*) as Found from {Table(options)} where {whereClause}";
        }

        public static string GetInsert(DMLOptions options = null)
        {
            return $"insert into {Table(options)}({Columns(options)}) values({Parameters(options)})";
        }
        
        public static string GetUpdate(string updateClause, string whereClause, DMLOptions options = null)
        {
            return $"update {Table(options)} set {updateClause} where {whereClause}";
        }

        public static string GetUpdate(object updateObject, object whereObject, DMLOptions options = null)
        {
            return $"update {Table(options)} set {updateObject.GetUpdateClause(options: options)} where {whereObject.GetWhereClause(options: options)}";
        }

        public static string GetDelete(object whereObject, DMLOptions options = null)
        {
            return $"delete from {Table(options)} where {whereObject.GetWhereClause(options: options)}";
        }
    }
}
