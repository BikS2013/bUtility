## bUtility
simple utilities. [nuget packages](https://www.nuget.org/packages?q=butility)
####Extensions:
``` c#
string Clear(this string value)
```
returns null for null or empty strings, otherwise returns the trimmed value.

``` c#
bool In<T>(this T value, params T[] list)
```
returns true if the array contains the value, false if the value or array is null, false otherwise.

``` c#
string Concatenate(this IEnumerable<string> list, Func<string, string, string> pattern)

//example: 
//stringList.Concatenate( (c,n) => $"{c}, {n}");
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

**GetPropertyNames**
``` c#
IEnumerable<string> GetPropertyNames(this Type type, Func<PropertyInfo, Boolean> filter = null)
IEnumerable<string> GetPropertyNames(this object obj, Func<PropertyInfo, Boolean> filter = null)
```

**GetMemberInfo**
``` c#
T GetMemberInfo<T>(this Type type, string memberName) where T : MemberInfo
T GetMemberInfo<T>(this object obj, string memberName) where T : MemberInfo
```
returns PropertyInfo, MethodInfo, etc from object or type for the member: memberName

**GetCustomAttribute**
```c#
T GetCustomAttribute<T>(this MemberInfo memberInfo) where T : System.Attribute
```
returns the custom attribute of type T if exist otherwise null

``` c#
```


## [bUtility.Dapper](docs/butility.dapper.md)
Dapper related utilities:
####Extension functions:
