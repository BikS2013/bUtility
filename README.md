#### [bUtility.Dapper](docs/butility.dapper.md), [bUtility.Dapper.Sql](docs/butility.dapper.sql.md), [bUtility.Dapper.Oracle](docs/butility.dapper.oracle.md), [bUtility.WebApi](docs/butility.WebApi.md), [bUtility.Sts](docs/butility.sts.md), [bUtility.ReverseProxy](butility.ReverseProxy.md)


## bUtility

####Extensions:
**Clear**
``` c#
string Clear(this string value)
```

**In**
``` c#
bool In<T>(this T value, params T[] list)
```

**Concatenate**
``` c#
string Concatenate(this IEnumerable<string> list, Func<string, string, string> pattern)

//example: 
//stringList.Concatenate( (c,n) => $"{c}, {n}");
```

**HasAny**
``` c#
bool HasAny<T>(this IEnumerable<T> collection)
```

####Reflection Extensions:
**GetInstance**
``` c#
object GetInstance(this Type type, params object[] parameters)
```

**GetValue**
``` c#
object GetValue(this object obj, string propertyName)
T GetValue<T>(this object obj, string propertyName, T defaultValue) where T : class
```


**SetValue**
``` c#
void SetValue(this object obj, string propertyName, object value)
```

**Invoke**
``` c#
void Invoke(this object obj, string methodName, params object[] parameters)
```

**GetMembers**
``` c#
IEnumerable<T> GetMembers<T>(this Type type, Func<T, Boolean> filter = null) where T :MemberInfo
```

**GetMemberNames**
``` c#
IEnumerable<string> GetMemberNames<T>(this Type type, Func<T, Boolean> filter = null) where T :MemberInfo
IEnumerable<string> GetMemberNames<T>(this object obj, Func<T, Boolean> filter = null) where T :MemberInfo
```

**GetMemberInfo**
``` c#
T GetMemberInfo<T>(this Type type, string memberName) where T : MemberInfo
T GetMemberInfo<T>(this object obj, string memberName) where T : MemberInfo
```

**GetCustomAttribute**
```c#
T GetCustomAttribute<T>(this MemberInfo memberInfo) where T : System.Attribute
```


``` c#
```


