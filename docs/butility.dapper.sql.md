#### [bUtility](../README.md), [bUtility.Dapper](butility.dapper.md), [bUtility.Dapper.Oracle](butility.dapper.oracle.md), [bUtility.WebApi](butility.WebApi.md), [bUtility.Sts](butility.sts.md), [bUtility.ReverseProxy](butility.ReverseProxy.md)

## bUtility.Dapper.Sql
Dapper related utilities for SqlServer specific implementations:


**BulkInsertSql**
```c#
int BulkInsertSql<T>(this IDbConnection con, IEnumerable<T> dataList)
```