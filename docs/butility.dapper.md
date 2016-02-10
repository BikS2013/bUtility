## bUtility.Dapper
Dapper related utilities:
####Extension functions:

```c#
string GetParameterList(this string columnList)
string GetParameterList(this Type type)
```

```c#
string GetColumnList(this Type type)
```

```c#
string GetUpdateColumnList(this Type type, Func<PropertyInfo, Boolean> filter = null)
```

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

