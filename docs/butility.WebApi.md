#### [bUtility](../README.md), [bUtility.Sts](butility.sts.md), [bUtility.Dapper](butility.dapper.md), [bUtility.Dapper.Sql](butility.dapper.sql.md), [bUtility.Dapper.Oracle](butility.dapper.oracle.md), [bUtility.ReverseProxy](butility.ReverseProxy.md)

## bUtility.WebApi

###Routing Configuration
**Global.asax** 
```c#
GlobalConfiguration.Configure((httpConfiguration) =>
{

    httpConfiguration.Routes.MapHttpRoute(
            name: "DefaultPage",
            routeTemplate: "",
            defaults: new { controller = "Index", action = "Get" }
        );

    //other routes
    //....


    //generic api routing
    httpConfiguration.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{action}"
        );

    //handles any other route
    httpConfiguration.Routes.MapHttpRoute(
        name: "Error404",
        routeTemplate: "{*url}",
        defaults: new { controller = "Index", action = "Get" }
    );

    //handles wrong urls & exceptions
    httpConfiguration.Services.Replace(typeof(IHttpControllerSelector), 
        new ControllerSelector(httpConfiguration, "Index", "GetSimple"));
    httpConfiguration.Services.Replace(typeof(IHttpActionSelector), 
        new ActionSelector( ()=> { return new IndexController();  }, 
        "GetSimple"));
});

```

