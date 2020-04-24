#### [bUtility](../README.md), [bUtility.WebApi](butility.WebApi.md), [bUtility.Sts](butility.sts.md), [bUtility.Dapper](butility.dapper.md), [bUtility.Dapper.Sql](butility.dapper.sql.md), [bUtility.Dapper.Oracle](butility.dapper.oracle.md)

## bUtility.ReverseProxy
Reverse Proxy utility

###Initialization\
Creates a reverse proxy that routes requests from source url to target url using HttpClient.\
The HttpClient must have been preconfigured from the calling app. This function should never return a new instance of the HttpClient.

webApiDestinationUrl => source url\
webApiDestinationUrl => destination url\
client => the HttpClient\
requestMessage.PrepareProxyRequest() => function that adds header(s) to the request (e.g. authentication headers, custom headers e.t.c..)

```c#
var proxy = new ReverseProxyHandler(webApiSourceUrl, webApiDestinationUrl,
                client,
                requestMessage => requestMessage.PrepareProxyRequest(),
                (response) => { }, Logger);

```

