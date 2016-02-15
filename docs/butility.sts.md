#### [bUtility](../README.md), [bUtility.Dapper](butility.dapper.md)

## bUtility.sts
Sts related utilities:

####Constants: 
Federation, TokenTypes


####Configuration classes: 

RelyingParty, RelyingParties, StsConfiguration

(are used to describe STS server configuration, supported relying parties etc.)


**RelyingParty** members
```c#
[ConfigurationProperty("name", IsKey = true, IsRequired = true)]
public string Name

[ConfigurationProperty("tokenLifeTime")]
public long TokenLifeTime

[ConfigurationProperty("redirectUrl")]
public string RedirectUrl

[ConfigurationProperty("realm")]
public string Realm

[ConfigurationProperty("authenticationUrl")]
public string AuthenticationUrl

[ConfigurationProperty("issuerName")]
public string IssuerName

[ConfigurationProperty("tokenType")]
public string TokenType

[ConfigurationProperty("encryptingCertificate")]
public CertificateReferenceElement EncryptingCertificate

[ConfigurationProperty("signingCertificate")]
public CertificateReferenceElement SigningCertificate

```


####Sts Implementation: 
**RequestScope** constructor
```c#
RequestScope(Uri uri, RelyingParty rp)
```

**SimpleStsConfiguration** constructor
```c#
SimpleStsConfiguration(RelyingParty rp)
```


**SimpleSts** implementations
```c#
override Scope GetScope(ClaimsPrincipal principal, RequestSecurityToken request)
override Lifetime GetTokenLifetime(Lifetime requestLifetime)
override ClaimsIdentity GetOutputClaimsIdentity(ClaimsPrincipal principal, RequestSecurityToken request, Scope scope)
```

####Sts Usage
**RelyingPartyExtensions**
```c#
SignInRequestMessage GetSignInRequestMessage(this RelyingParty rp, Uri baseUri)
SimpleStsConfiguration GetStsConfiguration(this RelyingParty rp)
SignInResponseMessage ProcessSignInRequest(this RelyingParty rp, Uri baseUri, ClaimsPrincipal principal)
void HandleSignIn(this HttpResponse httpResponse, Uri baseUri, RelyingParty rp, ClaimsPrincipal principal)
```


###Sample Applications: 

####bUtility.Sts.MvcSample: sample sts implementation 
**Web Config Details**
```c#
  <configSections>
    <section name="bUtility.Sts" type="bUtility.Sts.Configuration.StsConfiguration, bUtility.Sts, Version=0.0.0.1, Culture=neutral" />
    <section name="system.identityModel" type="System.IdentityModel.Configuration.SystemIdentityModelSection, System.IdentityModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=B77A5C561934E089" />
  </configSections>
```

**STS.Config Details**
```c#
<?xml version="1.0"?>
<bUtility.Sts>
  <relyingParties>
    <rp name="test" tokenLifeTime="480" issuerName="simpleSTS"
        redirectUrl="https://localhost/bUtility.Sts.MvcClient/Sample/index" 
        realm="https://localhost/bUtility.Sts.MvcClient/" 
        authenticationUrl="http://localhost/bUtility.Sts.MvcSample/account"
        tokenType="urn:oasis:names:tc:SAML:1.0:assertion">
      <signingCertificate storeLocation="LocalMachine" storeName="My" 
                          x509FindType="FindBySubjectName" 
                          findValue="issuer.model.local" />
    </rp>
  </relyingParties>
</bUtility.Sts>
```




```c#

```
```c#

```

