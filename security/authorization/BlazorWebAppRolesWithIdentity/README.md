# Roles Sample App (Blazor Web App with ASP.NET Core Identity)

This sample app demonstrates how to use role-based and policy-based authorization with ASP.NET Core.

For more information, see [Role-based authorization in ASP.NET Core](https://learn.microsoft.com/aspnet/core/security/authorization/roles).

> [!CAUTION]
> This sample app uses an in-memory database to store user information, which isn't suitable for production scenarios. The sample app is intended for demonstration purposes only and shouldn't be used as a starting point for production apps.

## Steps to run the sample

1. Clone this repository or download a ZIP archive of the repository. For more information, see [How to download a sample](https://learn.microsoft.com/aspnet/core/introduction-to-aspnet-core#how-to-download-a-sample).

1. If you plan to run the app using the .NET CLI with `dotnet run`, note that the first launch profile in the launch settings file is used to run an app, which is the insecure `http` profile (HTTP protocol). To run the app securely (HTTPS protocol), take ***either*** of the following approaches:

   * Pass the launch profile option to the command when running the app: `dotnet run -lp https`.
   * In the launch settings file (`Properties/launchSettings.json`), rotate the `https` profile to the top, placing it above the `http` profile.
  
   If you use Visual Studio to run the app, Visual Studio automatically uses the `https` launch profile. No action is required to run the app securely when using Visual Studio.

1. Run the app.

1. Sign into the app using any of the following accounts to demonstrate the role-based and policy-based authorization features of the app that match the examples in the article:

   * `leela@contoso.com` (Password: `Passw0rd!`). Leela has `Admin` and `SuperUser` roles.
   * `harry@contoso.com` (Password: `Passw0rd!`). Harry only has the `Admin` role.
   * `sarah@contoso.com` (Password: `Passw0rd!`). Sarah only has the `SuperUser` role.
