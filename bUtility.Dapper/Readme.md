## bUtility.Dapper
Dapper related utilities:
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
T SelectSingle<T>(this IDbConnection con, object whereObject, IDbTransaction trn = null, bool buffered=true, int? timeout = 0, CommandType? commandType = null)
IEnumerable<T> Select<T>(this IDbConnection con, object whereObject, IDbTransaction trn = null, bool buffered = true, int? timeout = 0, CommandType? commandType = null)
```
> the `T` type name must be identical to the name of the target table

> the `whereObject`'s property names and values, are used in order to compose the where statement

> the query "result set" will be used in order to populate objects of type T, filling their properties with values. 

> The matching between the result set's column and the object's property is based on their names.

**Insert**
```c#
int Insert<T>(this IDbConnection con, T data, IDbTransaction trn=null, int? timeout=0, CommandType? commandType=null)
```

**Delete**
```c#
int Delete<T>(this IDbConnection con, object whereObject, IDbTransaction trn = null, int? timeout = 0, CommandType? commandType = null)
```
> the `T` type name must be identical to the name of the table

> the `whereObject`'s property names and values are used in order to compose the where statement
