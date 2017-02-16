#### [bUtility](../README.md), [bUtility.Dapper](butility.Dapper.md), [bUtility.Dapper.Sql](butility.dapper.sql.md), [bUtility.WebApi](butility.WebApi.md), [bUtility.Sts](butility.sts.md)

## bUtility.Dapper.Oracle
Dapper related utilities for OracleDb specific implementations:


**BulkInsertOracleDb**
```c#
int BulkInsertOracleDb<T>(this IDbConnection con, IEnumerable<T> dataList)