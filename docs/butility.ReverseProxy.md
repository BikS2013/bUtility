#### [bUtility](../README.md), [bUtility.WebApi](butility.WebApi.md), [bUtility.Sts](butility.sts.md), [bUtility.Dapper](butility.dapper.md), [bUtility.Dapper.Sql](butility.dapper.sql.md), [bUtility.Dapper.Oracle](butility.dapper.oracle.md)

## bUtility.ReverseProxy
Reverse Proxy utility

###Initialization
Creates a reverse proxy that routes requests from source url to target url using HttpClient.
Accepts a function (e.g. GetClient below) that returns an HttpClient. The client must have been preconfigured from the calling app. This function should never return a new instance of the HttpClient.

webApiDestinationUrl => source url
webApiDestinationUrl => destination url
GetClient() => function that returns a preconfigured httpclient which should be intiliazied once only. Meaning that the calling app must tace care of creating only one httpclient per reverseproxy.
requestMessage.PrepareProxyRequest() => function that adds header to the request e.g. to pass authentication headers

```c#
var proxy = new ReverseProxyHandler(webApiSourceUrl, webApiDestinationUrl,
                () => GetClient(),
                requestMessage => requestMessage.PrepareProxyRequest(),
                (response) => { }, Logger);

```

