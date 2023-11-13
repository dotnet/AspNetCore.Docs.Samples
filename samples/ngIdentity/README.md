# Angular client with ASP.NET Core identity APIs

This sample project demonstrates how to use ASP.NET Core Identity APIs with an Angular client. The project was generated using Angular hosted on an ASP.NET Core 8 template, then modified to add identity.

## Run the sample

Either run or inspect the launch settings for your projects. "Client" refers to the `ngIdentity.client` project and "server" refers to the `ngIdentity.server` host project. 

* Modify `proxy.conf.js` and ensure the URL and port are correct for your server project. 
* Modify `home.component.html` to post to the server URL.
* Run the project

> Sometimes a race condition can occur where the Angular build lags behind the server. The server  automatically kicks off an Angular build if it can't connect to the client, so this results in two instances of the Angular build. The second Angular build then fails with a port conflict. When this happens, close the _first_ Angular client to launch and the second will succeed. If you close the second, it will end the debug session.

## Understand the sample

The identity APIs were added to the server project, then the client was modified to use the APIs.

### Server

The following changes were made to the server project to support authentication and authorization:

1. Added the `Microsoft.AspNetCore.Identity.EntityFrameworkCore` NuGet package.
1. Added the `Microsoft.EntityFrameworkCore.InMemory` NuGet package. You can use any relational EF Core provider, but you will need to properly create/migrate the database (see [Migrations Overview](https://learn.microsoft.com/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli)).
1. Added the identity user and database.
   ```csharp
   class MyUser : IdentityUser { }

   class AppDbContext(DbContextOptions<AppDbContext> options) :
      IdentityDbContext<MyUser>(options)
   {    
   } 
   ```
1. Configured the identity services. The code here changes the default behavior of returning a `404` when not authenticated to a `401` so the client can automatically redirect to the login page.
   ```csharp
   builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies()
    .ApplicationCookie!.Configure(opt => opt.Events = new CookieAuthenticationEvents()
    {
        OnRedirectToLogin = ctx =>
        {
            ctx.Response.StatusCode = 401;
            return Task.CompletedTask;
        }
    }); 
    builder.Services.AddAuthorizationBuilder();
   ```
1. Configured the EF Core `DbContext` to use the in-memory identity database, and turned on the identity APIs.
   ```csharp
   builder.Services.AddDbContext<AppDbContext>(
      options => options.UseInMemoryDatabase("AppDb"));

   builder.Services.AddIdentityCore<MyUser>()
     .AddEntityFrameworkStores<AppDbContext>()
     .AddApiEndpoints();
   ```
1. Mapped the identity API endpoints: `app.MapIdentityApi<MyUser>();`
1. Added a logout method. This isn't included in the endpoints because it may change based on your configuration (i.e. cookies, tokens, both). More on this method below.
1. Locked down the weather forecast API to only respond to authenticated users by adding the extension: `.RequireAuthorization();`

The `logout` endpoint is defined like this:

```csharp
app.MapPost("/logout", async (
    SignInManager<MyUser> signInManager,
    [FromBody]object empty) =>
{
    if (empty is not null)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }
    return Results.NotFound();
}).RequireAuthorization();
```

Minimal APIs allow you to bind to forms. When you bind to forms, you risk exposing your site to cross-site request forgery (CSRF) attacks. To prevent this, you use the [anti-forgery services](https://learn.microsoft.com/aspnet/core/security/anti-request-forgery?view=aspnetcore-8.0). This logout doesn't bind to a form, but it also takes no parameters. Without parameters, an empty forms post from a malicious site could invoke the API. To avoid this, the client will post with an empty body: `{}`. The server checks for the parameter. If the empty body is included, the `empty` parameter is not null (it is deserialized as a `JObject`) and the method will succeed.

A forms post cannot produce the empty JSON body, so will either render a null parameter or throw a `415` (media type not supported). There is an option on the home page of the client app to try to log out using the cross-site forms post. 

### Client

The components to manage authentication are in the `identity` folder.

* **dto** defines the shape of parameters for the identity APIs.
* **guard** can be used with the router to prevent navigation to a route if the user is not authenticated. It will automatically redirect to the login page. It is used to protect the weather forecast route (although the server will also prevent it from being accessed by an unauthenticated user).
   ```json
   {
      path: 'forecast',
      component: ForecastComponent,
      canActivate: mapToCanActivate([AuthGuard])
  }
  ```
* **interceptor** is a convenience feature that responds to any `401` unauthorized by redirecting to the login page. Note that when you use roles or claims for authorization, a user could be logged in and receive a `401` when accessing a resource they don't have permissions for. This interceptor shouldn't be used in that scenario.
* **service** is the main service responsible for logging the user in and out and verifying whether the user is authenticated.

The other components use these services to restrict access. The common pattern is to inject `AuthService` and check if the is logged in. The client should also subscribe to `onStateChanged` to refresh anytime the user logs out or signs back in.

```typescript
public isSignedIn: boolean = false;

constructor(private authService: AuthService) { }

ngOnInit(): void {
  this.authService.onStateChanged().forEach((state: boolean) => {
    this.isSignedIn = state;      
  });
  this.authService.isSignedIn().forEach((signedIn: boolean) => {
    this.isSignedIn = signedIn;
  });
}
```

The component uses `*ngIf` to selectively render code based on the authentication state.
