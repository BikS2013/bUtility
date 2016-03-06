#### [bUtility](../README.md), [bUtility.Sts](butility.sts.md)

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
