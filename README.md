## bUtility
simple utilities. [nuget packages](https://www.nuget.org/packages?q=butility)
####Extension functions:
``` c#
string Clear(this string value)
```
returns null for null or empty strings, otherwise returns the trimmed value.

``` c#
bool In<T>(this T value, params T[] list)
```
returns true if the array contains the value, false if the value or array is null, false otherwise.

``` c#
object GetInstance(this Type type, params object[] parameters)
```
creates an instance of the specified type

``` c#
object GetPublic(this object obj, string propertyName)
```
returns the value of the public property "propertyName" for object

``` c#
T GetPublic<T>(this object obj, string propertyName, T defaultValue) where T : class
```

``` c#
void SetPublic(this object obj, string propertyName, object value)
```

``` c#
void Invoke(this object obj, string methodName, params object[] parameters)
```

``` c#
T GetCustomAttribute<T>(this MethodInfo methodInfo) where T : System.Attribute
```

## bUtility.Dapper
Dapper related utilities:
####Extension functions:
