# SimpleWebAuth
This is an authentication project that use JWT Tokens for .NET Framework Web API and MVC projects. A future improvement is to expand this project to allow other types of authentication (not only JWT tokens).

.NET Core version coming soon!

## Installation
This project will be added to NuGet gallery soon. For now, download this solution and add its project to your target solution.

## How to Use
In the **startup.cs** class or any initial configuration that your web project executes, add the following code to use SimpleWebAuth authentication with JWT (JSON web token).

```C#
SimpleWebAuth.WebAuthConfig.RegisterEndpoints(
  httpConfiguration,
  swaggerRequestsWithNoAuth: true,
  routesWithNoAuth: GetRoutesWithNoAuth());
```

The first parameter, **httpConfiguration**, is an instance of **System.Web.Http.HttpConfiguration**. This is necessary because SimpleWebAuth project adds two routes for login and token generation. The routes will be explained later. 

The second paramenter is necessary when your project has Swagger (https://swagger.io/). This indicates whether Swagger requests will require authentication or not.

The third parameter is a list of all routes that should not be authenticated. The following code gets all **System.Web.Http.ApiController** methods with the **[AllowAnonymous]** attribute.

```C#
private List<string> GetRoutesWithNoAuth()
{
    List<string> routesWithNoAuth = new List<string>();

    Assembly asm = Assembly.GetExecutingAssembly();
    List<MethodInfo> anonymousRoutes = asm.GetTypes()
        .Where(type => typeof(ApiController).IsAssignableFrom(type))
        .SelectMany(type => type.GetMethods())
        .Where(method => method.IsDefined(typeof(AllowAnonymousAttribute)))
        .ToList();

    foreach (MethodInfo methodInfo in anonymousRoutes)
    {
        RouteAttribute routeAttribute = methodInfo.GetCustomAttribute<RouteAttribute>(true);
        if (routeAttribute != null)
            routesWithNoAuth.Add(routeAttribute.Template);
    }

    return routesWithNoAuth;
}
```

It is possible to choose how this web auth project will validate the user information passed in the attempt to login. It can be done using the **Active Directory (AD)** or in any other way. This configuration is done by adding the following code in your startup class.

```C#
SimpleWebAuth.WebAuthConfig.RegisterUserValidation(SimpleWebAuth.UserValidationType.Custom, CustomUserValidation);
```

The fisrt parameter is an Enum that indicates which type of user validation will be done. If the chosen value is **UserValidationType.AD** the second optional parameter is not needed. The SimpleWebAuth project will validade the current windows user in the AD. If the chosen value is **UserValidationType.Custom**, then the second parameter is required. The second parameter is a delegate function responsible for validating user information passed in the attempt to login. The following code is an example.

```C#
public TokenUser CustomUserValidation(string userName = null, string password = null)
{
    if (String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(password))
        throw new ApplicationException("Invalid user name or password");

    if (userName != "rodrigo.bernardino")
        throw new ApplicationException("Invalid user name");

    if(password != "Test@123")
        throw new ApplicationException("Invalid password");

    var userClaimsTypes = new ConcurrentDictionary<string, string>();
    userClaimsTypes.GetOrAdd("UserName", userName);
    userClaimsTypes.GetOrAdd("UserRole", "Admin");

    return new TokenUser
    {
        UserName = userName,
        ClaimTypesValues = userClaimsTypes
    };
}
```

As mentioned before, the SimpleWebAuth project creates two routes for login and token generation: **auth/windowsLogin** and **auth/formsLogin**. The first one will uses the built in AD validation, so it gets the current windows user, validates it in the AD and returns a JWT token for it. The second one uses the custom user validation delegate function. It expects the user name and password and validates this information calling the delegate function.

The SimpleWebAuth project creates a **DelegatingHandler** that is responsible for getting the token from each request header and validating it. This **DelegatingHandler** puts the JWT user in the **Thread.CurrentPrincipal** and **HttpContext.Current.User**. Only routes configured to not be authenticated will skip this token validation.

If you want to disable authentication, you can use this code in the startup class. This is useful in some development environments.

```C#
SimpleWebAuth.WebAuthConfig.DisableTokenValidation();
```

This projest does not deal with any **AuthorizeAttribute**. Therefore, you can create your own **AuthorizeAttribute** without any problems.
