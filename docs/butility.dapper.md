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

**GetWhereClause4[Not]Nulls**
```c#
string GetWhereClause4NotNulls(this object obj)
string getWherePart4Nulls(this object obj)

IEnumerable<string> GetWhereParts(this object obj)
string getWherePart(this object obj)
```

```c#
IEnumerable<T> execQuery<T>(this IDbConnection con, object param)
```
```c#

```
```c#

```

