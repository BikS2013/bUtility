## bUtility.sts
Sts related utilities:

####Constants: 
Federation, TokenTypes


####Configuration classes: 
RelyingParty, RelyingParties, StsConfiguration
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



```c#

```
```c#

```

