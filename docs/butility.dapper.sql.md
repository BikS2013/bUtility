#### [bUtility](../README.md), [bUtility.Dapper](butility.Dapper.md), [bUtility.Dapper.Oracle](butility.Dapper.Oracle.md), [bUtility.WebApi](butility.WebApi.md), [bUtility.Sts](butility.sts.md)

## bUtility.Dapper.Sql
Dapper related utilities for SqlServer specific implementations:


**BulkInsertSql**
```c#
int BulkInsertSql<T>(this IDbConnection con, IEnumerable<T> dataList)
```