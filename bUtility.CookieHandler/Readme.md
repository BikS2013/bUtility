[google docs version](https://docs.google.com/document/d/1xVPEvJqlBSIjmlhujshW9BaXE0UV22hHfo8gf2qsvRU/edit?usp=sharing)

##bUtility.CookieHandler

In WS-Federation scenarios, we use the [SessionAuthenticationModule](https://msdn.microsoft.com/en-us/library/system.identitymodel.services.sessionauthenticationmodule(v=vs.110).aspx) class in order to:
- handle the initial SecurityToken post, 
- capture the posted security token, 
- and replace it by a cookie which will be the session cookie. 

This cookie translation task is managed by the [CookieHandler](https://msdn.microsoft.com/en-us/library/system.identitymodel.services.sessionauthenticationmodule.cookiehandler(v=vs.110).aspx) part (property) of the SessionAuthenticationModule.
The default cookie handler implementation used in this case, is the ChunkedCookieHandler implemented in System.IdentityModel.Services.
This one produces secure or non secure cookies based on the requireSsl value of the system.identityModel.services configuration.

```
<?xml version="1.0"?>
<system.identityModel.services >
  <federationConfiguration >
    <cookieHandler requireSsl="true"/>
    <wsFederation passiveRedirectEnabled="true" issuer="..." realm="..." requireHttps="true" />
  </federationConfiguration>
</system.identityModel.services>
```

The problem is that setting the requireSsl = “true”, WS-Federation requires the communication channel to be ssl encrypted. This requirement is perfectly reasonable, except of the cases when the implementation resides behind a load balancer responsible to offload the ssl protocol. 
In this case the http requests are delivered to the web application in plain html, and the System.IdentityModel.Services library thows the … exception.

```
<?xml version="1.0"?>
<system.identityModel.services >
  <federationConfiguration >
    <cookieHandler mode="Custom" requireSsl="false">
      <customCookieHandler 
             type="bUtility.CookieHandler.SecureChunkedCookieHandler, bUtility.CookieHandler" />
    </cookieHandler>
    <wsFederation passiveRedirectEnabled="true"  issuer="..." realm="..." requireHttps="false" />
  </federationConfiguration>
</system.identityModel.services>
```

The bUtility.CookieHadler.SecureChunkedCookieHandler is an option in order to bypass this limitation. Based on the original implementation of ChunkedCookieHandler it always generates secure cookies, regardless the value of requireSsl option.

[WSFederationAuthenticationModule Class](https://msdn.microsoft.com/en-us/library/system.identitymodel.services.sessionauthenticationmodule(v=vs.110).aspx)

[SessionAuthenticationModule Class](https://msdn.microsoft.com/en-us/library/system.identitymodel.services.sessionauthenticationmodule.cookiehandler(v=vs.110).aspx)
