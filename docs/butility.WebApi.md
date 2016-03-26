#### [bUtility](../README.md), [bUtility.Sts](butility.sts.md), [bUtility.Dapper](butility.dapper.md)

## bUtility.WebApi

###NotFoundResolution
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


    //default api route
    httpConfiguration.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{action}"
        );

    //any other route
    httpConfiguration.Routes.MapHttpRoute(
        name: "Error404",
        routeTemplate: "{*url}",
        defaults: new { controller = "Index", action = "Get" }
    );

    //exception handling
    httpConfiguration.Services.Replace(typeof(IHttpControllerSelector), 
        new ControllerSelector(httpConfiguration, "Index", "GetSimple"));
    httpConfiguration.Services.Replace(typeof(IHttpActionSelector), 
        new ActionSelector( ()=> { return new IndexController();  }, 
        "GetSimple"));
});

```

