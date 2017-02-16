#### [bUtility](../README.md), [bUtility.Dapper.Sql](butility.Dapper.Sql.md), [bUtility.Dapper.Oracle](butility.Dapper.Oracle.md), [bUtility.WebApi](butility.WebApi.md), [bUtility.Sts](butility.sts.md)

## bUtility.Dapper
Dapper related utilities:

**DMLOptions**

Options for query contruction, including identifier (tables and columns) delimeters and parameter delimeters.
Can be set to ```DMLOptions.CurrentOptions``` for static use.

Eg:
* for SqlServer ```DMLOptions.DefaultOptions``` can be used (or left null)
* for OracleDB ```new DMLOptions(':')```, if case sensitive ```new DMLOptions(':', '"')```

####Extension functions:

**GetColumnList**
```c#
string GetColumnList(this Type type)
```

**GetParameterList**
```c#
string GetParameterList(this string columnList)
string GetParameterList(this Type type)
```

**GetUpdateClause**
```c#
string GetUpdateClause(this Type type, Func<PropertyInfo, Boolean> filter = null)
```

**GetWhereClause**
```c#
string GetWhereClause(this object obj, boolean includeNulls=false)
```

```c#
IEnumerable<T> execQuery<T>(this IDbConnection con, object param)
```

**Select**
```c#
IEnumerable<T> Select<T>(this IDbConnection con)
T SelectSingle<T>(this IDbConnection con, object whereObject)
IEnumerable<T> Select<T>(this IDbConnection con, object whereObject)
```

**Insert**
```c#
int Insert<T>(this IDbConnection con, T data)
```

**Delete**
```c#
int Delete<T>(this IDbConnection con, object whereObject)
```

**Update**
```c#
int Update<T>(this IDbConnection con, object updateObject, object whereObject)
```

**MultipleInsert**
```c#
int MultipleInsert<T>(this IDbConnection con, IEnumerable<T> dataList)
```